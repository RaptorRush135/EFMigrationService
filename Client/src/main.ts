import { EventToPromiseConverter } from "./event-to-promise-converter";
import { TerminalEmulator } from "./terminal-emulator";
import * as signalR from "@microsoft/signalr";

const terminal = new TerminalEmulator(document.getElementById("terminal")!);

window.addEventListener("resize", () => terminal.fit());

let serverUrl: string | undefined = process.env.SERVER_URL;

if (!serverUrl) {
  serverUrl = await terminal.prompt("Server URL: ");
} else {
  terminal.writeLine(`Server URL: ${serverUrl}`);
}

terminal.writeLine("- Migration service -");

const hub = new signalR.HubConnectionBuilder()
  .withUrl(serverUrl)
  .withAutomaticReconnect()
  .configureLogging(signalR.LogLevel.Information)
  .build();

hub.on("ReceiveLine", (line: string) => {
  terminal.writeLine(line);
});

const endOfResponseEvent = new EventToPromiseConverter<void>();

hub.on("EndOfResponse", () => {
  endOfResponseEvent.handleEvent();
});

try {
  await hub.start();
} catch (error: unknown) {
  terminal.writeLine(String(error));
  throw error;
}

await endOfResponseEvent.createPromise();

while (true) {
  const command: string = await terminal.prompt("dotnet tool run dotnet-ef ");
  terminal.newLine();

  const endOfResponsePromise = endOfResponseEvent.createPromise();

  await hub.invoke("ReceiveCommand", command);

  await endOfResponsePromise;
}

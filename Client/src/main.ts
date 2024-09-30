import { EventToPromiseConverter } from './event-to-promise-converter';
import { TerminalEmulator } from './terminal-emulator';
import * as signalR from "@microsoft/signalr";

const serverUrl = process.env.SERVER_URL;

if (!serverUrl) {
  throw new Error("SERVER_URL not defined");
}

console.log("Server url: " + serverUrl);

const terminal = new TerminalEmulator(document.getElementById('terminal')!, "dotnet tool run dotnet-ef ");

window.addEventListener('resize', () => terminal.fit());

terminal.writeLine("- Migration service -");

const hub = new signalR.HubConnectionBuilder()
  .withUrl(serverUrl + "/terminal")
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

await hub.start();

await endOfResponseEvent.createPromise();

while (true) {
  const command: string = await terminal.prompt();
  terminal.newLine();

  const endOfResponsePromise = endOfResponseEvent.createPromise();

  await hub.invoke("ReceiveCommand", command);

  await endOfResponsePromise;
}

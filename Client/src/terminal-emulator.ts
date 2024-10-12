import { Terminal } from "@xterm/xterm";
import { FitAddon } from "@xterm/addon-fit";
import { LineBuffer } from "./line-buffer";
import { EventToPromiseConverter } from "./event-to-promise-converter";

export class TerminalEmulator {
  private readonly terminal: Terminal;
  private readonly fitAddon: FitAddon;
  private readonly lineBuffer: LineBuffer;
  private readonly newLineEvent: EventToPromiseConverter<string> =
    new EventToPromiseConverter();
  private inputEnabled: boolean = false;

  public constructor(readonly container: HTMLElement) {
    this.terminal = new Terminal({ cursorBlink: true });

    this.fitAddon = new FitAddon();
    this.terminal.loadAddon(this.fitAddon);

    this.terminal.open(container);

    this.fitAddon.fit();

    this.listenToInput();

    this.lineBuffer = new LineBuffer(this.terminal);
  }

  public writeLine(data: string): void {
    this.terminal.write(data);
    this.newLine();
  }

  public newLine(): void {
    this.terminal.write("\r\n");
  }

  public async prompt(prompt: string): Promise<string> {
    await this.writePrompt(prompt);
    this.lineBuffer.updateCursorOffset(prompt.length);
    this.inputEnabled = true;

    return this.newLineEvent.createPromise();
  }

  public fit(): void {
    this.fitAddon.fit();
  }

  private onNewLine() {
    this.inputEnabled = false;
    this.newLine();

    const buffer: string = this.lineBuffer.buffer;
    this.lineBuffer.clear();

    this.newLineEvent.handleEvent(buffer);
  }

  private writePrompt(prompt: string): Promise<void> {
    this.newLine();

    return new Promise((resolve) => {
      this.terminal.write(prompt, () => {
        resolve();
      });
    });
  }

  private listenToInput(): void {
    this.terminal.onData((data: string) => {
      if (!this.inputEnabled) {
        return;
      }

      const multiline = data.indexOf("\r") >= 0;

      data = data.split("\r")[0];

      switch (data) {
        case "\r":
        case "\u0003":
          this.onNewLine();
          return;
        case "\u0008":
        case "\u007F":
          this.lineBuffer.backspace();
          break;
        case "\x1b[A": // Up arrow
        case "\x1b[B": // Down arrow
          break;
        case "\x1b[C": // Right arrow
          this.lineBuffer.cursorPos++;
          break;
        case "\x1b[D": // Left arrow
          this.lineBuffer.cursorPos--;
          break;
        default:
          this.lineBuffer.write(data);
      }

      if (multiline) {
        this.onNewLine();
      }
    });
  }
}

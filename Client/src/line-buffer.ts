import { Terminal } from "@xterm/xterm";

export class LineBuffer {
  private line: string = "";

  private cursorOffset: number = 0;

  public constructor(private readonly terminal: Terminal) {}

  public get buffer(): string {
    return this.line;
  }

  public updateCursorOffset(cursorOffset: number): void {
    this.cursorOffset = cursorOffset;
  }

  public async write(data: string): Promise<void> {
    if (data.length == 0 || data.charCodeAt(0) == 27) {
      return;
    }

    data = [...data]
      .filter((ch) => {
        const code = ch.charCodeAt(0);
        return code >= 32 && code <= 126;
      })
      .join("");

    const originalCursorPos = this.cursorPos;

    this.rawWrite(data);
    const rightText = this.line.slice(originalCursorPos);
    await this.rawWritePromise(rightText);

    this.cursorPos = originalCursorPos + 1;
    this.line = this.line.slice(0, originalCursorPos) + data + rightText;
  }

  public async backspace(): Promise<void> {
    const originalCursorPos = this.cursorPos;
    if (originalCursorPos <= 0) {
      return;
    }

    const rightText = this.line.slice(originalCursorPos);

    this.rawWrite("\b \b");
    this.rawWrite(rightText);
    await this.rawWritePromise(" ");

    const newCursorPos = originalCursorPos - 1;
    this.cursorPos = newCursorPos;
    this.line = this.line.slice(0, newCursorPos) + rightText;
  }

  public clear(): void {
    this.line = "";
  }

  public get cursorPos(): number {
    return this.terminal.buffer.active.cursorX - this.cursorOffset;
  }

  public set cursorPos(value: number) {
    if (!this.isInCursoBounds(value)) {
      return;
    }

    const offset = value - this.cursorPos;
    const writeData = offset >= 0 ? "\x1b[C" : "\x1b[D";
    for (let count = 0; count < Math.abs(offset); count++) {
      this.rawWrite(writeData);
    }
  }

  private isInCursoBounds(value: number) {
    return value >= 0 && value <= this.line.length;
  }

  private rawWrite(data: string, callback?: () => void): void {
    this.terminal.write(data, callback);
  }

  private rawWritePromise(data: string): Promise<void> {
    return new Promise((resolve) => {
      this.rawWrite(data, () => {
        resolve();
      });
    });
  }
}

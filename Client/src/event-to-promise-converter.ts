export class EventToPromiseConverter<T> {
  private resolver: ((value: T) => void) | null = null;

  public createPromise(): Promise<T> {
    return new Promise<T>((resolve) => {
      this.resolver = resolve;
    });
  }

  public handleEvent(value: T): void {
    if (this.resolver != null) {
      this.resolver(value);
      this.resolver = null;
    }
  }
}

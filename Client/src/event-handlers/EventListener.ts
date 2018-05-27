export class EventListener {
    private name: string
    private callback: (args: any[]) => void

    constructor(eventName: string, callback: (args: any[]) => void) {
        this.name = eventName
        this.callback = callback
    }

    public run(args: any[]): void {
        this.callback(args)
    }

    public getName(): string { return this.name }
}
import { EventListener } from './EventListener'

export class EventHandler {
    private listeners: {[name: string]: EventListener[]} = {}

    // Создать и добавить обработчик
    public on(eventName: string, callback: (args: any[]) => void): void {
        let listener: EventListener = new EventListener(eventName, callback)
        this.add(listener)
    }

    // Добавить обработчик
    public add(listener: EventListener): void {
        if (!this.listeners[listener.getName()]) {
            this.listeners[listener.getName()] = new Array<EventListener>()
        }
        this.listeners[listener.getName()].push(listener)
    }

    // Удалить обработчик
    public remove(listener: EventListener): void {
        this.listeners[listener.getName()].filter((e: EventListener) => {
            return e !== listener
        })
    }

    // Удалить обработчики по имени группы
    public removeAll(eventName: string): void {
        this.listeners[eventName] = new Array<EventListener>()
    }
    
    // Вызвать обработчик
    public trigger (eventName: string, args: any[]): void {
        if (!this.listeners[eventName]) {
            return
        }
        this.listeners[eventName].forEach((e: EventListener) => {
            e.run(args)
        })
    }
}
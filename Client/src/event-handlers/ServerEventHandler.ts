/// <reference path="../../types-gt-mp/index.d.ts" />

import { EventHandler } from './EventHandler'
import { ConvertToArray } from '../utils/ArgumentConverter'

export class ServerEventHandler extends EventHandler {
    private static instance: ServerEventHandler

    private constructor() { super() }

    public static getInstance(): ServerEventHandler {
        if (!ServerEventHandler.instance) {
            ServerEventHandler.instance = new ServerEventHandler()
        }
        return ServerEventHandler.instance
    }
}

API.onServerEventTrigger.connect((eventName: string, serverArgs: System.Array<any>) => {
    let args = ConvertToArray(serverArgs)
    ServerEventHandler.getInstance().trigger(eventName, args)
})
/// <reference path='../../types-gt-mp/index.d.ts' />

import { EventHandler } from '../event-handlers/EventHandler'

export class CefEventHandlerSingleton extends EventHandler {
  private static instance: CefEventHandlerSingleton

  private constructor () {
    super()
  }

  public static getInstance (): CefEventHandlerSingleton {
    if (!CefEventHandlerSingleton.instance) {
      CefEventHandlerSingleton.instance = new CefEventHandlerSingleton()
    }
    return CefEventHandlerSingleton.instance
  }
}

// Hack for GT-MP - this makes everything callable outside of webpack, so in the gloabal context
API.onResourceStart.connect(() => {
  resource.CefEventHandlerSingleton = CefEventHandlerSingleton.getInstance()
})
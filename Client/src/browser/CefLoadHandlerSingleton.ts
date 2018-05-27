/// <reference path='../../types-gt-mp/index.d.ts' />
/// <reference path='./Cef.ts' />

export class CefLoadHandlerSingleton {
    private static instance: CefLoadHandlerSingleton
    private loadingCallbacks: { [name: string]: () => void } = {}
  
    private constructor () {}
  
    public static getInstance (): CefLoadHandlerSingleton {
      if (!CefLoadHandlerSingleton.instance) CefLoadHandlerSingleton.instance = new CefLoadHandlerSingleton()
      return CefLoadHandlerSingleton.instance
    }
  
    public doneLoading (name: string): void {
      this.loadingCallbacks[name]()
    }
  
    public finishedLoading (name: string): Promise<void> {
      return new Promise<void>((resolve: () => void) => {
        this.loadingCallbacks[name] = resolve
      })
    }
  }
  
  // Hack for GT-MP - this makes everything callable outside of webpack, so in  the gloabal context
  API.onResourceStart.connect(() => {
    resource.CefLoadHandlerSingleton = CefLoadHandlerSingleton.getInstance()
  })
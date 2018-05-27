/// <reference path='../../../types-gt-mp/index.d.ts' />

import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'

const rotationThreshold = 0.001

export class VoiceManager {
    private static instance: VoiceManager
    private browser: GrandTheftMultiplayer.Client.GUI.CEF.Browser
    private handshakeTimer = -1;
    private rotationTimer = -1;
    private lastRotation = 0;

    private constructor() {       
        this.buildBrowser()
        ServerEventHandler.getInstance().on('VoiceSetHandshake', this.onVoiceSetHandshake)
    }

    public static getInstance() {
        if (!VoiceManager.instance) {
            VoiceManager.instance = new VoiceManager()
        }
        return VoiceManager.instance
    }
 
    private buildBrowser() {
        this.browser = API.createCefBrowser(0, 0, false);
        API.waitUntilCefBrowserInit(this.browser);
        API.setCefBrowserHeadless(this.browser, true);
    }

    private onVoiceSetHandshake(args: any[]) {
        if (args.length === 2) {
            VoiceManager.instance.setHandshake(args[0], args[1]);
        } else {
            VoiceManager.instance.setHandshake(args[0], '');
        }
    }

    public sendHandshake(url: string) {
        API.loadPageCefBrowser(this.browser, url);
    }

    public sendRotation() {
        const rotation = ((API.getGameplayCamRot().Z * -1) * Math.PI) / 180;
        if (Math.abs(this.lastRotation - rotation) < rotationThreshold) {
            return;
        }
        API.triggerServerEvent("ChangeVoiceRotation", rotation);
        this.lastRotation = rotation;
    }

    private setHandshake(status: boolean, url: string) {
        if (status === (this.handshakeTimer !== -1)) {
            return;
        }        
        if (status) {
            if (this.rotationTimer !== -1) {
                API.stop(this.rotationTimer);
                this.rotationTimer = -1;
            }

            this.handshakeTimer = API.every(3000, "resendHandshake", url);
            resendHandshake(url);
        } else {
            if (this.handshakeTimer !== -1) {
                API.stop(this.handshakeTimer);
                this.handshakeTimer = -1;
            }

            this.rotationTimer = API.every(333, "sendCamrotation");
        }
    }

    private dispose() {
        if (this.handshakeTimer !== -1) {
            API.stop(this.handshakeTimer);
            this.handshakeTimer = -1;
        }
        if (this.rotationTimer !== -1) {
            API.stop(this.rotationTimer);
            this.rotationTimer = -1;
        }
    }
}

function resendHandshake(url: string) {
    let instance = VoiceManager.getInstance()
    if (!instance) {
        return;
    }    
    instance.sendHandshake(url);
}

function sendCamrotation() {
    let instance = VoiceManager.getInstance()
    if (!instance) {
        return;
    }
    instance.sendRotation();
}
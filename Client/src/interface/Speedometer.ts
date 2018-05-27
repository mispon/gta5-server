/// <reference path='../../types-gt-mp/index.d.ts' />

import { Cef } from '../browser/Cef'
import { ServerEventHandler } from '../event-handlers/ServerEventHandler'

export class Speedometer {
    private static instance: Speedometer
    private cef: Cef
    private inVehicle: boolean
    private playersVehicle: LocalHandle
    private showSvg: boolean = false
    private updateTimeout: number
    private lastUpdate: number = Date.now()
    private lastFuelUpdate: number = Date.now()
    private defaultSpeedometer: string = 'ui/interface/speedometer/index.html'
    private svgSpeedometer: string = 'ui/interface/svg-speedometer/index.html'

    private constructor() {
        this.updateTimeout = 700
        this.cef = new Cef('Speedometer', this.defaultSpeedometer)
        this.cef.setChatVisible(true)
        this.cef.setHeadless(true)
        this.cef.load()
        ServerEventHandler.getInstance().on('ShowSpeedometer', this.showSpeedometer)
        ServerEventHandler.getInstance().on('HideSpeedometer', this.hideSpeedometer)
    }

    public static getInstance (): Speedometer {
        if (!Speedometer.instance) {
            Speedometer.instance = new Speedometer()
        }
        return Speedometer.instance
    }

    public update() {
        if (!Speedometer.instance.inVehicle) {
            return
        }
        if (!Speedometer.instance.needUpdate()) {
            return
        }        
        Speedometer.instance.updateSpeed(Speedometer.instance.playersVehicle)
        if (Speedometer.instance.showSvg) {
            Speedometer.instance.updateRpm(Speedometer.instance.playersVehicle)
        }
        Speedometer.instance.updateFuel(Speedometer.instance.playersVehicle)
    }

    private showSpeedometer(args: any[]) {
        Speedometer.instance.showSvg = args[0] as boolean
        Speedometer.instance.updateTimeout = Speedometer.instance.showSvg ? 250 : 1000
        Speedometer.instance.playersVehicle = API.getPlayerVehicle(API.getLocalPlayer())
        Speedometer.instance.loadSpeedometer()
        Speedometer.instance.inVehicle = true
    }

    private hideSpeedometer() {
        Speedometer.instance.inVehicle = false
        Speedometer.instance.cef.destroy()
    }

    private async updateSpeed(vehicle: LocalHandle) {
        let velocity = API.getEntityVelocity(vehicle)
        let speed = Math.sqrt(
            velocity.X * velocity.X +
            velocity.Y * velocity.Y +
            velocity.Z * velocity.Z
        )
        await Speedometer.instance.cef.eval(`updateSpeed(${speed * 3.6})`);
    }

    private async updateRpm(vehicle: LocalHandle) {
        let rpm = API.getVehicleRPM(vehicle);
        await Speedometer.instance.cef.eval(`updateRpm(${rpm * 10})`);
    }

    private async updateFuel(vehicle: LocalHandle) {
        let duration = Date.now() - Speedometer.instance.lastFuelUpdate
        if (duration < 10000) {
            return
        }
        let fuel = API.getVehicleFuelLevel(vehicle)
        await Speedometer.instance.cef.eval(`updateFuel(${fuel})`)
        Speedometer.instance.lastFuelUpdate = Date.now()
    }

    private loadSpeedometer() {        
        let path = Speedometer.instance.showSvg 
            ? Speedometer.instance.svgSpeedometer 
            : Speedometer.instance.defaultSpeedometer
        Speedometer.instance.createCef(path)
    }

    private createCef(path: string) {
        Speedometer.instance.cef = new Cef('Speedometer', path)
        Speedometer.instance.cef.setChatVisible(true)
        Speedometer.instance.cef.load()
    }

    private needUpdate(): boolean {
        let duration = Date.now() - Speedometer.instance.lastUpdate
        if (duration < Speedometer.instance.updateTimeout) {
            return false
        }
        Speedometer.instance.lastUpdate = Date.now()
        return true
    }
}

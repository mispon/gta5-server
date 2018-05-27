/// <reference path='../../../types-gt-mp/index.d.ts' />

import { PoliceAlert } from '../../models/PoliceAlert'
import { VectorConverter } from '../../utils/VectorConverter'
import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'

namespace PoliceAlertManager {
    let alerts: Map<number, LocalHandle> = new Map()

    ServerEventHandler.getInstance().on('ShowPoliceAlerts', showAlerts)
    ServerEventHandler.getInstance().on('HidePoliceAlerts', hideAlerts)
    ServerEventHandler.getInstance().on('CreatePoliceAlert', createAlert)
    ServerEventHandler.getInstance().on('UpdatePoliceAlert', updateAlert)
    ServerEventHandler.getInstance().on('FinishPoliceAlert', finishAlert)

    function showAlerts(args: any[]) {
        let allAlerts = JSON.parse(args[0]) as PoliceAlert[]
        allAlerts.forEach(alert => {
            addAlert(alert)
        });
    }

    function hideAlerts() {
        let alertIds = Array.from(alerts.keys())
        alertIds.forEach(key => {
            finishAlert([key])
        })
    }
    
    function createAlert(args: any[]) {
        let alert = JSON.parse(args[0]) as PoliceAlert
        addAlert(alert)
    }

    function updateAlert(args: any[]) {
        let alert = JSON.parse(args[0]) as PoliceAlert
        let blip = alerts.get(alert.Id) as LocalHandle
        let position = VectorConverter.convert(alert.Position)
        setBlipStyle(blip, position, alert.Name)
    }

    function finishAlert(args: any[]) {
        let alertId = args[0] as number
        if (!alerts.has(alertId)) {
            return
        }
        let blip = alerts.get(alertId) as LocalHandle
        API.deleteEntity(blip)      
        alerts.delete(alertId)
    }

    function addAlert(alert: PoliceAlert) {
        let position = VectorConverter.convert(alert.Position)
        alerts.set(alert.Id, createBlip(position, alert.Name))
    }

    function createBlip(position: Vector3, name: string): LocalHandle {
        let blip = API.createBlip(position)
        setBlipStyle(blip, position, name)
        return blip
    }

    function setBlipStyle(blip: LocalHandle, position: Vector3, name: string) {
        API.setBlipSprite(blip, getSprite(name))
        API.setBlipColor(blip, 1)
        API.setBlipName(blip, name)
    }

    function getSprite(name: string) {
        switch(name) {
            case 'Убийство':
                return 378
            case 'Драка':
                return 491
            default:
                return 1
        }
    }
}
/// <reference path='../../../types-gt-mp/index.d.ts' />

import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'

namespace PilotManager { 
    let currentMarker: LocalHandle
    let currentBlip: LocalHandle

    ServerEventHandler.getInstance().on('ShowPilotTargetPoint', showTargetPoint)
    ServerEventHandler.getInstance().on('HidePilotTargetPoint', hideTargetPoint)

    API.onKeyDown.connect((sender, args) => {
        let player = API.getLocalPlayer()
        if (args.KeyCode != Keys.E || !API.hasEntitySyncedData(player, 'IsPilot')) {
            return
        }
        API.triggerServerEvent('ProcessPilotDelivery')
    })

    function showTargetPoint(args: any[]): void {
        let position = args[0] as Vector3
        currentMarker = API.createMarker(1, position, new Vector3(), new Vector3(), new Vector3(8, 8, 8), 16, 125, 161, 255)
        createBlip(position)
    }

    function createBlip(position: Vector3) {
        currentBlip = API.createBlip(position)
        API.setBlipSprite(currentBlip, 1)
        API.setBlipColor(currentBlip, 3)
        API.setBlipFlashing(currentBlip, true)
        API.setBlipName(currentBlip, 'Активный контракт')
    }

    function hideTargetPoint(args: any[]): void {
        API.deleteEntity(currentMarker)
        API.deleteEntity(currentBlip)
    }
}
/// <reference path='../../../types-gt-mp/index.d.ts' />

import { VectorConverter } from '../../utils/VectorConverter'
import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'

namespace TruckerManager { 
    let currentMarker: LocalHandle

    ServerEventHandler.getInstance().on('ShowTruckerTargetPoint', showTargetPoint)
    ServerEventHandler.getInstance().on('HideTruckerTargetPoint', hideTargetPoint)

    API.onKeyDown.connect((sender, args) => {
        if (args.KeyCode != Keys.O) {
            return
        }
        let player = API.getLocalPlayer()
        if (API.hasEntitySyncedData(player, 'IsTrucker') && (currentMarker != null && !currentMarker.IsNull)) {
            var position = API.getEntityPosition(currentMarker)
            API.setWaypoint(position.X, position.Y)
        }
    })

    function showTargetPoint(args: any[]): void {
        let position = args[0] as Vector3
        currentMarker = API.createMarker(1, position, new Vector3(), new Vector3(), new Vector3(4, 4, 4), 16, 125, 161, 255)
        API.setWaypoint(position.X, position.Y)
    }

    function hideTargetPoint(args: any[]): void {
        API.deleteEntity(currentMarker)
        API.removeWaypoint()
    }
}
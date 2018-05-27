import { GPS } from '../menu/GPS'
import { ServerEventHandler } from '../event-handlers/ServerEventHandler'

namespace GpsManager {
    let gps: GPS = new GPS()

    API.onKeyDown.connect((entity: any, args: System.Windows.Forms.KeyEventArgs) => {
        let player = API.getLocalPlayer()
        if (API.hasEntitySyncedData(player, 'DisableHotkeys')) {        
            return
        }
        if (args.KeyCode == Keys.O) {
            gps.triggerVisible()
        }
    })

    ServerEventHandler.getInstance().on('ShowGPSTarget', showTarget)
    ServerEventHandler.getInstance().on('ShowGPSMenu', showGPS)
    ServerEventHandler.getInstance().on('HideGPSMenu', hideGPS)

    function showTarget(args: any[]) {
        let target = args[0] as Vector3
        API.setWaypoint(target.X, target.Y)
    }

    function showGPS() { gps.show() }
    function hideGPS() { gps.hide() }
}


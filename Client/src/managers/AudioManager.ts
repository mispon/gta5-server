import { ServerEventHandler } from '../event-handlers/ServerEventHandler'

namespace AudioManager {
    ServerEventHandler.getInstance().on('CarLockSound', carLock)

    function carLock() {
        try {
            API.startAudio('sounds/car-lock.wav', false)  
        } catch(e) {}
    }
}
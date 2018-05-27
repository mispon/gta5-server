/// <reference path='../../../types-gt-mp/index.d.ts' />

import { DressingRoom } from '../../models/DressingRoom'
import { ActiveCamera } from '../../utils/ActiveCamera'

export class DressingCameraManager {
    private lastCameraIndex: number = -1
    private headCamera: GrandTheftMultiplayer.Client.Javascript.GlobalCamera 
    private topsCamera: GrandTheftMultiplayer.Client.Javascript.GlobalCamera
    private legsCamera: GrandTheftMultiplayer.Client.Javascript.GlobalCamera
    private feetsCamera: GrandTheftMultiplayer.Client.Javascript.GlobalCamera

    constructor(dressingRoom: DressingRoom) {
        this.headCamera = ActiveCamera.createCamera(dressingRoom.CameraHatsPosition, dressingRoom.CameraRotation)
        this.topsCamera = ActiveCamera.createCamera(dressingRoom.CameraTopsPosition, dressingRoom.CameraRotation)
        this.legsCamera = ActiveCamera.createCamera(dressingRoom.CameraLegsPosition, dressingRoom.CameraRotation)
        this.feetsCamera = ActiveCamera.createCamera(dressingRoom.CameraFeetsPosition, dressingRoom.CameraRotation)
    }

    public moveCamera(index: number) {
        let camera = API.getActiveCamera()
        switch(index) {
            case 0:
            if (this.lastCameraIndex == index) {
                return
            }
            API.interpolateCameras(camera, this.headCamera, 500, true, false)
            this.lastCameraIndex = index
            break
            case 1:
            if (this.lastCameraIndex == index) {
                return
            }                    
            API.interpolateCameras(camera, this.topsCamera, 500, true, false)
            this.lastCameraIndex = index
            break
            case 2:
            if (this.lastCameraIndex == index) {
                return
            }
            API.interpolateCameras(camera, this.legsCamera, 500, true, false)
            this.lastCameraIndex = index
            break
            case 3:
            if (this.lastCameraIndex == index) {
                return
            }
            API.interpolateCameras(camera, this.feetsCamera, 500, true, false)
            this.lastCameraIndex = index
            break
        }
    }
}
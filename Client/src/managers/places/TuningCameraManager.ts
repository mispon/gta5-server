/// <reference path='../../../types-gt-mp/index.d.ts' />

import { ActiveCamera } from '../../utils/ActiveCamera'

export class TuningCameraManager {    
    private sideSlots: number[] = [3, 5, 15, 23, 24]
    private backSlots: number[] = [0, 2, 4, 8, 39]

    private frontCamera: GrandTheftMultiplayer.Client.Javascript.GlobalCamera 
    private sideCamera: GrandTheftMultiplayer.Client.Javascript.GlobalCamera
    private backCamera: GrandTheftMultiplayer.Client.Javascript.GlobalCamera

    constructor() {
        this.frontCamera = ActiveCamera.createCamera(new Vector3(197.10, -999.55, -98.00), new Vector3(-20.00, -5.00, -64.00))
        this.sideCamera = ActiveCamera.createCamera(new Vector3(201.56, -1002.63, -98.80), new Vector3(-5.00, 0.50, 0.00))
        this.backCamera = ActiveCamera.createCamera(new Vector3(205.51, -999.00, -99.00), new Vector3(0.00, 0.00, 67.90))        
    }

    public setFrontCamera() {
        API.setActiveCamera(this.frontCamera)
    }

    public moveCamera(slot: number) {        
        if (this.sideSlots.indexOf(slot) !== -1) {
            API.setActiveCamera(this.sideCamera)
        }
        else if (this.backSlots.indexOf(slot) !== -1) {
            API.setActiveCamera(this.backCamera)
        } else {
            API.setActiveCamera(this.frontCamera)
        }
    }
}
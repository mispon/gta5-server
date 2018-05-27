export class ActiveCamera {
    public static setActiveCamera(position: Vector3, rotation: Vector3) {
        let camera = this.createCamera(position, rotation)
        API.setActiveCamera(camera)
    }

    public static createCamera(position: Vector3, rotation: Vector3): GrandTheftMultiplayer.Client.Javascript.GlobalCamera {
        return API.createCamera(new Vector3(position.X, position.Y, position.Z), new Vector3(rotation.X, rotation.Y, rotation.Z))
    }
}
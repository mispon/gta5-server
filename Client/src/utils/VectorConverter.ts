export class VectorConverter {
    // note: сомнительный конвертер из-за проблем с векторами на клиенте
    public static convert(value: Vector3): Vector3 {
        return new Vector3(value.X, value.Y, value.Z)
    }
}
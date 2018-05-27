declare namespace GTA.Math {

	class Matrix {
		Item: number;
		readonly IsIdentity: boolean;
		readonly HasInverse: boolean;
		constructor(values: any[]);
		Determinant(): number;
		Det3x3(M11: number, M12: number, M13: number, M21: number, M22: number, M23: number, M31: number, M32: number, M33: number): number;
		Invert(): void;
		TransformPoint(vector: GTA.Math.Vector3): GTA.Math.Vector3;
		InverseTransformPoint(vector: GTA.Math.Vector3): GTA.Math.Vector3;
		ToArray(): any[];
		ToString(): string;
		ToString(format: string): string;
		GetHashCode(): number;
		Equals(obj: any): boolean;
		Equals(other: GTA.Math.Matrix): boolean;
	}

	class Quaternion {
		readonly Axis: GTA.Math.Vector3;
		readonly Angle: number;
		constructor(x: number, y: number, z: number, w: number);
		constructor(axis: GTA.Math.Vector3, angle: number);
		Length(): number;
		RotateTransform(point: GTA.Math.Vector3): GTA.Math.Vector3;
		RotateTransform(point: GTA.Math.Vector3, center: GTA.Math.Vector3): GTA.Math.Vector3;
		ToString(): string;
		ToString(format: string): string;
		GetHashCode(): number;
		Equals(obj: any): boolean;
		Equals(other: GTA.Math.Quaternion): boolean;
	}

	class Vector3 {
		readonly Normalized: GTA.Math.Vector3;
		Item: number;
		constructor(x: number, y: number, z: number);
		Length(): number;
		LengthSquared(): number;
		Normalize(): void;
		DistanceTo(position: GTA.Math.Vector3): number;
		DistanceToSquared(position: GTA.Math.Vector3): number;
		TransformCoordinate(transform: any): void;
		DistanceTo2D(position: GTA.Math.Vector3): number;
		DistanceToSquared2D(position: GTA.Math.Vector3): number;
		ToHeading(): number;
		Around(distance: number): GTA.Math.Vector3;
		ToString(): string;
		ToString(format: string): string;
		GetHashCode(): number;
		Equals(obj: any): boolean;
		Equals(other: GTA.Math.Vector3): boolean;
	}

}

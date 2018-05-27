declare namespace System {

	class Array<T> implements System.Collections.IList {
		[index: number]: T;
		readonly Length: number;
		readonly LongLength: number;
		readonly Rank: number;
		readonly Count: number;
		readonly SyncRoot: any;
		readonly IsReadOnly: boolean;
		readonly IsFixedSize: boolean;
		readonly IsSynchronized: boolean;
		readonly Item: any;
		GetValue(...indices: any[]): any;
		GetValue(index: number): any;
		GetValue(index1: number, index2: number): any;
		GetValue(index1: number, index2: number, index3: number): any;
		GetValue(index: number): any;
		GetValue(index1: number, index2: number): any;
		GetValue(index1: number, index2: number, index3: number): any;
		GetValue(...indices: any[]): any;
		SetValue(value: any, index: number): void;
		SetValue(value: any, index1: number, index2: number): void;
		SetValue(value: any, index1: number, index2: number, index3: number): void;
		SetValue(value: any, ...indices: any[]): void;
		SetValue(value: any, index: number): void;
		SetValue(value: any, index1: number, index2: number): void;
		SetValue(value: any, index1: number, index2: number, index3: number): void;
		SetValue(value: any, ...indices: any[]): void;
		GetLength(dimension: number): number;
		GetLongLength(dimension: number): number;
		GetUpperBound(dimension: number): number;
		GetLowerBound(dimension: number): number;
		Add(value: any): number;
		Contains(value: any): boolean;
		Clear(): void;
		IndexOf(value: any): number;
		Insert(index: number, value: any): void;
		Remove(value: any): void;
		RemoveAt(index: number): void;
		Clone(): any;
		CompareTo(other: any, comparer: any): number;
		Equals(other: any, comparer: any): boolean;
		GetHashCode(comparer: any): number;
		CopyTo(array: System.Array<T>, index: number): void;
		CopyTo(array: System.Array<T>, index: number): void;
		GetEnumerator(): any;
		Initialize(): void;
	}

	class AsyncCallback {
		constructor(object: any, method: any);
		Invoke(ar: System.IAsyncResult): void;
		BeginInvoke(ar: System.IAsyncResult, callback: System.AsyncCallback, object: any): System.IAsyncResult;
		EndInvoke(result: System.IAsyncResult): void;
	}

	class EventArgs {
		constructor();
	}

	interface IAsyncResult {
		readonly IsCompleted: boolean;
		readonly AsyncWaitHandle: any;
		readonly AsyncState: any;
		readonly CompletedSynchronously: boolean;
	}

	class TimeSpan {
		readonly Ticks: number;
		readonly Days: number;
		readonly Hours: number;
		readonly Milliseconds: number;
		readonly Minutes: number;
		readonly Seconds: number;
		readonly TotalDays: number;
		readonly TotalHours: number;
		readonly TotalMilliseconds: number;
		readonly TotalMinutes: number;
		readonly TotalSeconds: number;
		constructor(ticks: number);
		constructor(hours: number, minutes: number, seconds: number);
		constructor(days: number, hours: number, minutes: number, seconds: number);
		constructor(days: number, hours: number, minutes: number, seconds: number, milliseconds: number);
		Add(ts: System.TimeSpan): System.TimeSpan;
		CompareTo(value: any): number;
		CompareTo(value: System.TimeSpan): number;
		Duration(): System.TimeSpan;
		Equals(value: any): boolean;
		Equals(obj: System.TimeSpan): boolean;
		GetHashCode(): number;
		Negate(): System.TimeSpan;
		Subtract(ts: System.TimeSpan): System.TimeSpan;
		ToString(): string;
		ToString(format: string): string;
		ToString(format: string, formatProvider: any): string;
	}

}

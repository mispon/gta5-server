export function ConvertToArray(args: System.Array<any>): any[] {
    let result: any[] = []
    for (let i = 0; i < args.Length; i++) {
        result.push(args[i]);       
    }
    return result
}
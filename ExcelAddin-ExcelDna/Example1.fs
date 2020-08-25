namespace Addin

open System
open ExcelDna.Integration
open ExcelDna.Registration.FSharp

 
module Example1 =

    let rnd = System.Random(Seed = 1)


    let createTimer (interval: float) =
        let timer = new System.Timers.Timer(interval)
        timer.AutoReset <- true
        timer.Start()
        timer.Elapsed

    let performOnEvent event (f: unit -> 'b) : IObservable<'b>=
        event
        |> Observable.map (fun _ -> f ())
    
    let testAsync () =
        async {
            do! Async.Sleep 1000
            let res = rnd.Next(0,1000)
            return res
        }

    let funTest () =
        Async.RunSynchronously (testAsync ())

    [<ExcelFunction(Description = "Test", IsThreadSafe = false)>]
    let testObservable (str: string) =
        FsAsyncUtil.excelObserve "test" [|str|]
            (performOnEvent (createTimer 5000.) funTest)
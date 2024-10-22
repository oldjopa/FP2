module PropertyTests
// open FsCheck
// open FsCheck.Xunit

// open bt_bag

// type YourType = int

// type BTreeArbitrary() =
//     static member Arb =
//         Arb.fromGen (Gen.choose (0, 100) |> Gen.map (fun x -> insert x))

// // Свойство: ассоциативность
// let associativeMerge (t1: BTreeMultiset<'a>) (t2: BTreeMultiset<'a>) (t3: BTreeMultiset<'a>) =
//     let merged1 = mergeTrees (mergeTrees t1 t2) t3
//     let merged2 = mergeTrees t1 (mergeTrees t2 t3)
//     merged1 = merged2

// [<Property>]
// let ``Merge is associative`` () =
//     Prop.forAll BTreeArbitrary.Arb (fun _ (t1, t2, t3) -> associativeMerge t1 t2 t3)

// // Свойство: идентичность
// let identityMerge tree =
//     let mergedWithEmpty1 = mergeTrees tree Empty
//     let mergedWithEmpty2 = mergeTrees Empty tree
//     tree = mergedWithEmpty1 && tree = mergedWithEmpty2

// [<Property>]
// let ``Merge with empty tree returns the same tree`` () =
//     Prop.forAll BTreeArbitrary.Arb (fun _ tree -> identityMerge tree)

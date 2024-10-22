module Tests

open System
open Xunit
open FsCheck
open FsCheck.Xunit

open bt_bag


let tree =
    Empty |> insert 5 |> insert 3 |> insert 7 |> insert 3 |> insert 6 |> insert 8


[<Fact>]
let ``Insert should add elements correctly`` () =
    let result = countElement 3 tree
    Assert.Equal(2, result)

[<Fact>]
let ``Remove should decrease count`` () =
    let updatedTree = remove 3 tree
    Assert.Equal(1, countElement 3 updatedTree)

[<Fact>]
let ``Remove should remove element if count is 1`` () =
    let updatedTree = remove 3 tree |> remove 3
    Assert.Equal(0, countElement 3 updatedTree)


[<Fact>]
let ``Remove root element should restructure tree correctly`` () =
    let updatedTree = remove 5 tree
    Assert.Equal(0, countElement 5 updatedTree)
    Assert.Equal(2, countElement 3 updatedTree)
    Assert.Equal(1, countElement 6 updatedTree)

[<Fact>]
let ``Remove non-existent element should not change tree`` () =
    let updatedTree = remove 4 tree
    Assert.Equal(2, countElement 3 updatedTree)
    Assert.Equal(1, countElement 7 updatedTree)

[<Fact>]
let ``Merge trees with overlapping elements`` () =
    let tree1 = Empty |> insert 5 |> insert 3 |> insert 7
    let tree2 = Empty |> insert 7 |> insert 8 |> insert 5
    let mergedTree = mergeTrees tree1 tree2
    Assert.Equal(2, countElement 5 mergedTree)
    Assert.Equal(2, countElement 7 mergedTree)
    Assert.Equal(1, countElement 3 mergedTree)
    Assert.Equal(1, countElement 8 mergedTree)

[<Fact>]
let ``Complex tree removal and merge`` () =
    let tree1 = Empty |> insert 10 |> insert 5 |> insert 15 |> insert 10
    let tree2 = Empty |> insert 20 |> insert 15 |> insert 25
    let updatedTree1 = remove 10 tree1
    let mergedTree = mergeTrees updatedTree1 tree2
    Assert.Equal(1, countElement 10 mergedTree)
    Assert.Equal(2, countElement 15 mergedTree)
    Assert.Equal(1, countElement 20 mergedTree)
    Assert.Equal(1, countElement 25 mergedTree)

[<Fact>]
let ``Removing all occurrences of an element`` () =
    let updatedTree = remove 3 (remove 3 tree)
    Assert.Equal(0, countElement 3 updatedTree)
    Assert.Equal(1, countElement 5 updatedTree)
    Assert.Equal(1, countElement 6 updatedTree)
    Assert.Equal(1, countElement 8 updatedTree)



let tree2 = Empty |> insert 5 |> insert 5 |> insert 3 |> insert 7 |> insert 6 |> insert 8

[<Fact>]
let ``Fold should sum all elements correctly`` () =
    let sum = foldLeft (fun acc value -> acc + value) 0 tree2
    Assert.Equal((10 + 3 + 7 + 6 + 8), sum)

[<Fact>]
let ``Filter should return tree with elements greater than 4`` () =
    let filteredTree = filter (fun value -> value > 4) tree2
    let sum = foldLeft (fun acc value -> acc + value) 0 filteredTree
    Assert.Equal((10 + 7 + 6 + 8), sum)


///////// property-based тесты

// type BTreeArbitrary() =
//     static member Arb =
//         Arb.fromGen (Gen.choose (0, 100) |> Gen.map (fun x -> insert x) )


// // Свойство: ассоциативность
// let associativeMerge (t1: BTreeMultiset<'a>) (t2: BTreeMultiset<'a>) (t3: BTreeMultiset<'a>) =
//     let merged1 = mergeTrees (mergeTrees t1 t2) t3
//     let merged2 = mergeTrees t1 (mergeTrees t2 t3)
//     merged1 = merged2

// [<Property>]
// let ``Merge is associative`` () =
//     Prop.forAll BTreeArbitrary.Arb (fun _ t1 t2  t3 -> associativeMerge t1 t2 t3)

// // Свойство: идентичность
// let identityMerge tree =
//     let mergedWithEmpty1 = mergeTrees tree Empty
//     let mergedWithEmpty2 = mergeTrees Empty tree
//     tree = mergedWithEmpty1 && tree = mergedWithEmpty2

// [<Property>]
// let ``Merge with empty tree returns the same tree`` () =
//     Prop.forAll BTreeArbitrary.Arb (fun _ tree -> identityMerge tree)
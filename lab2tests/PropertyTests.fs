module PropertyTests
open Xunit
open FsCheck
open FsCheck.Xunit

open bt_bag

type BTreeArbitrary() =
    static member Arb =
        Arb.fromGen (Gen.choose (0, 100) |> Gen.map (fun x -> insert x))

// // Свойство: ассоциативность
// let associativeMerge (t1: BTreeMultiset<'a>) (t2: BTreeMultiset<'a>) (t3: BTreeMultiset<'a>) =
//     let merged1 = mergeTrees (mergeTrees t1 t2) t3
//     let merged2 = mergeTrees t1 (mergeTrees t2 t3)
//     merged1 = merged2

// [<Property>]
// let ``Merge is associative`` () =
//     Prop.forAll BTreeArbitrary.Arb (fun (t1, t2, t3) -> associativeMerge t1 t2 t3)

// // Свойство: идентичность
// let identityMerge tree =
//     let mergedWithEmpty1 = mergeTrees tree Empty
//     let mergedWithEmpty2 = mergeTrees Empty tree
//     tree = mergedWithEmpty1 && tree = mergedWithEmpty2

// [<Property>]
// let ``Merge with empty tree returns the same tree`` () =
//     Prop.forAll BTreeArbitrary.Arb (fun tree -> identityMerge tree)

// // Пропозиция: вставка элемента и затем вставка того же элемента должны давать одинаковое дерево
// let prop_insertId (n: int) (tree: BTreeMultiset<int>) =
//     let treeAfterInsert = insert n tree
//     let treeAfterDoubleInsert = insert n treeAfterInsert
//     treeAfterInsert = treeAfterDoubleInsert

// // Пропозиция: вставка элемента в пустое дерево и потом вставка этого элемента
// let prop_emptyInsert (n: int) =
//     let tree = Empty
//     let newTree = insert n tree
//     match newTree with
//     | BNode(Node(v, count), _, _) -> v = n && count = 1
//     | _ -> false

// // Запуск тестов
// Check.Quick prop_insertId
// Check.Quick prop_emptyInsert


type ArrayArbitrary() =
    static member Arb =
        Arb.fromGen (Gen.choose (50, 100) |>  Gen.map (fun length -> Gen.arrayOfLength length (Gen.choose (-50, 50))))

[<Property>]
let prop_emptyInsert (n: int) =
    let newTree = insert n Empty
    match newTree with
    | BNode(Node(v, count), _, _) -> v = n && count = 1
    | _ -> false



[<Property(Arbitrary = [| typeof<ArrayArbitrary> |])>]
let prop_associativity (n1: int[]) (n2: int[]) =
    // // List.map (fun x -> printfn "%d" x) n1 |> ignore
    let tree1 = Array.fold (fun acc x -> insert x acc) Empty n1
    let tree2 = Array.fold (fun acc x -> insert x acc) Empty n2
    let mergedTree = mergeTrees tree1 tree2
    let mergedTree2 = mergeTrees tree2 tree1
    compareTrees mergedTree mergedTree2
open System
open bt_bag


[<EntryPoint>]
let main (argv: string array) =

    let tree = Empty

    let tree =
        tree |> insert 2 |> insert 3 |> insert 3 |> insert 7 |> insert 4 |> insert 8

    printTree tree ""

    let tree = remove 3 tree
    printTree tree "+"

    let evenTree = filter (fun x -> x % 2 = 0) tree
    printTree evenTree "="

    let incrementedTree = map (fun x -> x + 10) tree
    printTree incrementedTree "-"

    let sum = foldRight (fun acc x -> x + acc) 0 tree

    let tree2 = Empty |> insert 6 |> insert 8 |> insert 3 |> insert 4
    let mergedTree = merge tree tree2
    printTree mergedTree ""



    let tree3 =
        Empty |> insert 5 |> insert 3 |> insert 7 |> insert 3 |> insert 6 |> insert 8

    printTree tree3 "-"
    let updatedTree = remove 5 tree3
    printTree updatedTree "="
    0

module bt_bag


type BTreeElement<'T> = Node of ('T * int)

type BTreeMultiset<'T when 'T: comparison> =
    | Empty
    | BNode of BTreeElement<'T> * BTreeMultiset<'T> * BTreeMultiset<'T>


let rec repeatOperation (n: int) f element acc =
    match n with
    | 0 -> acc
    | n -> repeatOperation (n - 1) f element (f acc element)



let rec insert x tree =
    match tree with
    | Empty -> BNode(Node(x, 1), Empty, Empty)
    | BNode(Node(value, count), left, right) when x < value -> BNode(Node(value, count), insert x left, right)
    | BNode(Node(value, count), left, right) when x > value -> BNode(Node(value, count), left, insert x right)
    | BNode(Node(value, count), left, right) -> BNode(Node(value, count + 1), left, right)


let rec insertNode value count tree =
    match tree with
    | Empty -> BNode(Node(value, count), Empty, Empty)
    | BNode(Node(v, c), left, right) when v = value -> BNode(Node(v, c + count), left, right)
    | BNode(Node(v, c), left, right) when v < value -> BNode(Node(v, c), left, insertNode value count right)
    | BNode(Node(v, c), left, right) -> BNode(Node(v, c), insertNode value count left, right)


let rec mergeTrees t1 t2 =
    match t1 with
    | Empty -> t2
    | BNode(Node(value, count), left, right) ->
        let mergedRight = mergeTrees right t2
        mergeTrees left (insertNode value count mergedRight)



let rec remove x tree =
    match tree with
    | Empty -> Empty
    | BNode(Node(value, count), left, right) when x < value -> BNode(Node(value, count), remove x left, right)
    | BNode(Node(value, count), left, right) when x > value -> BNode(Node(value, count), left, remove x right)
    | BNode(Node(value, 1), left, right) -> mergeTrees left right
    | BNode(Node(value, count), left, right) -> BNode(Node(value, count - 1), left, right)


let rec countElement x tree =
    match tree with
    | Empty -> 0
    | BNode(Node(value, count), left, right) when x < value -> countElement x left
    | BNode(Node(value, count), left, right) when x > value -> countElement x right
    | BNode(Node(_, count), _, _) -> count


let rec printTree tree indent =
    match tree with
    | Empty -> ()
    | BNode(Node(value, count), left, right) ->
        printfn "%s%d (count: %d)" indent value count
        printTree left (indent + "  ")
        printTree right (indent + "  ")


let rec map f tree =
    match tree with
    | Empty -> Empty
    | BNode(Node(value, count), left, right) -> BNode(Node(f value, count), map f left, map f right)


let rec foldLeft f acc tree =
    match tree with
    | Empty -> acc
    | BNode(Node(value, count), left, right) ->
        let acc' = repeatOperation count f value acc
        let acc'' = foldLeft f acc' left
        foldLeft f acc'' right

let rec foldRight f acc tree =
    match tree with
    | Empty -> acc
    | BNode(Node(value, count), left, right) ->
        let acc' = repeatOperation count f acc value
        let acc'' = foldRight f acc' right
        foldRight f acc'' left



let rec filter predicate tree =
    match tree with
    | Empty -> Empty
    | BNode(Node(value, count), left, right) ->
        let filteredLeft = filter predicate left
        let filteredRight = filter predicate right

        if predicate value then
            BNode(Node(value, count), filteredLeft, filteredRight)
        else
            mergeTrees filteredLeft filteredRight


let rec countElements (tree: BTreeMultiset<'T>) : int =
    match tree with
    | Empty -> 0
    | BNode(Node(_, count), left, right) -> count + countElements left + countElements right


let compareTrees (tree1: BTreeMultiset<'T>) (tree2: BTreeMultiset<'T>) : bool =
    let count1 = countElements tree1
    let count2 = countElements tree2

    if count1 <> count2 then
        false
    else
        let rec checkElements tree1 tree2 =
            match tree1 with
            | Empty -> true
            | BNode(Node(value, count), left, right) ->
                countElement value tree2 = count
                && checkElements left tree2
                && checkElements right tree2

        checkElements tree1 tree2

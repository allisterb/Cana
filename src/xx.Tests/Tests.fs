module Base

open System
open Xunit
open FsCheck.Xunit

[<Property>]
let ``Reverse of reverse of a list is the original list ``(xs:list<int>) =
  List.rev(List.rev xs) = xs

// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2014 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}

namespace WebSharper.UI.Next

/// A potentially time-varying or animated attribute list.
[<Sealed>]
type Attr =

    /// Sets a basic DOM attribute, such as `id` to a text value.
    static member Create : name: string -> value: string -> Attr

  // Note: Empty, Append, Concat define a monoid on Attr.

    /// Append on attributes.
    static member Append : Attr -> Attr -> Attr

    /// Concatenation on attributes.
    static member Concat : seq<Attr> -> Attr

    /// Empty attribute list.
    static member Empty : Attr

namespace WebSharper.UI.Next.Server

open Microsoft.FSharp.Quotations
open WebSharper.JavaScript
open WebSharper.UI.Next
open WebSharper.Html.Server

module Attr =

    val AsAttributes : Attr -> list<Html.Attribute>

    val Handler : event: string -> callback: (Expr<#Dom.Event -> unit>) -> Attr

namespace WebSharper.UI.Next.Client

open WebSharper.UI.Next

module Attr =

    /// Dynamic variant of Create.
    val Dynamic : name: string -> value: View<string> -> Attr

    /// Dynamically set a property of the DOM element.
    val DynamicProp : name: string -> value: View<'T> -> Attr

    /// Dynamic with a custom setter.
    val internal DynamicCustom : set: (Element -> 'T -> unit) -> value: View<'T> -> Attr

    /// Animated variant of Create.
    val Animated : name: string -> Trans<'T> -> view: View<'T> -> value: ('T -> string) -> Attr

    /// Sets a style attribute, such as `background-color`.
    val Style : name: string -> value: string -> Attr

    /// Dynamic variant of Style.
    val DynamicStyle : name: string -> value: View<string> -> Attr

    /// Animated variant of Style.
    val AnimatedStyle : name: string -> Trans<'T> -> view: View<'T> -> value: ('T -> string) -> Attr

    /// Sets an event handler, for a given event such as `click`.
    val Handler : name: string -> callback: (DomEvent -> unit) -> Attr

    /// Sets a CSS class.
    val Class : name: string -> Attr

    /// Sets a CSS class when the given view satisfies a predicate.
    val DynamicClass : name: string -> view: View<'T> -> apply: ('T -> bool) -> Attr

    /// Sets an attribute when a view satisfies a predicate.
    val DynamicPred : name: string -> predView: View<bool> -> valView: View<string> -> Attr

    /// Gets and sets the value of the element according to a Var.
    val Value : Var<'a> -> Attr when 'a : equality

/// Internals used in Doc.
module internal Attrs =

    /// Dynamic attributes.
    type Dyn

    /// Inserts static attributes and computes dynamic attributes.
    val Insert : Element -> Attr -> Dyn

    /// Synchronizes dynamic attributes.
    /// Exception: does not sync nodes that animate change transitions.
    /// Those synchronize when the relevant transition is played.
    val Sync : Element -> Dyn -> unit

    /// Dynamic updates of attributes.
    val Updates : Dyn -> View<unit>

    /// Check if currently animating a changing value.
    val HasChangeAnim : Dyn -> bool

    /// Check if can animate enter transitions.
    val HasEnterAnim : Dyn -> bool

    /// Check if can animate exit transitions.
    val HasExitAnim : Dyn -> bool

    /// Animate a changing value.
    val GetChangeAnim : Dyn -> Anim

    /// Animate enter transition.
    val GetEnterAnim : Dyn -> Anim

    /// Animate exit transition.
    val GetExitAnim : Dyn -> Anim

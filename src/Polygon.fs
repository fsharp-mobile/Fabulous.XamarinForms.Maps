﻿namespace Fabulous.XamarinForms.Maps

open System.Collections.Generic
open System.Runtime.CompilerServices
open Fabulous
open Fabulous.StackAllocatedCollections
open Fabulous.StackAllocatedCollections.StackList
open Fabulous.XamarinForms
open Xamarin.Forms.Maps

type IPolygon =
    inherit IMapElement

module Polygon =
    let WidgetKey = Widgets.register<Polygon> ()

    let FillColor = Attributes.defineBindableAppThemeColor Polygon.FillColorProperty

    let GeoPathList =
        Attributes.defineSimpleScalarWithEquality<Position list> "Polygon_GeoPath" (fun _ newValueOpt node ->
            let map = node.Target :?> Polygon

            match newValueOpt with
            | ValueNone -> map.Geopath.Clear()
            | ValueSome geoPaths -> geoPaths |> List.iter map.Geopath.Add)


[<AutoOpen>]
module PolygonBuilders =

    type Fabulous.XamarinForms.View with

        static member inline Polygon<'msg>(geoPaths: Position list) =
            WidgetBuilder<'msg, IPolygon>(
                Polygon.WidgetKey,
                AttributesBundle(StackList.one (Polygon.GeoPathList.WithValue(geoPaths)), ValueNone, ValueNone)
            )

[<Extension>]
type PolygonModifiers =
    [<Extension>]
    static member inline geoPaths(this: WidgetBuilder<'msg, #IPolygon>, value: Position list) =
        this.AddScalar(Polygon.GeoPathList.WithValue(value))

    [<Extension>]
    static member inline fillColor(this: WidgetBuilder<'msg, #IPolygon>, light: FabColor, ?dark: FabColor) =
        this.AddScalar(Polygon.FillColor.WithValue(AppTheme.create light dark))

    /// <summary>Link a ViewRef to access the direct Polygon control instance</summary>
    [<Extension>]
    static member inline reference(this: WidgetBuilder<'msg, IPolygon>, value: ViewRef<Polygon>) =
        this.AddScalar(ViewRefAttributes.ViewRef.WithValue(value.Unbox))

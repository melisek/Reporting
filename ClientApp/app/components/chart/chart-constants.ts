import { INameValue } from "../shared/shared-interfaces";
import { IChartOption } from "./chart";

export const chartTypes: INameValue[] = [
    {
        name: "Horizontal Bar Chart",
        value: 0,
    },
    {
        name: "Vertical Bar Chart",
        value: 1
    },
    {
        name: "Pie Chart",
        value: 2
    },
    {
        name: "Line Chart",
        value: 3
    }
];

export const aggregationTypes: INameValue[] =
[
    { name: "Sum", value: 0 },
    { name: "Average", value: 1 },
    { name: "Minimum", value: 2 },
    { name: "Maximum", value: 3 },
    { name: "Count", value: 4 },
    ];


export const baseOptions: IChartOption[] = [
    { name: "animations", text: "Enable animations", type: "boolean", value: true },
    { name: "showLegend", text: "Show legend", type: "boolean", value: true },
    { name: "legendTitle", text: "Legend title", type: "string", value: "Legend" },
    { name: "gradient", text: "Gradient", type: "boolean", value: true },
    { name: "tooltipDisabled", text: "Disable tooltip", type: "boolean", value: false }
    //scheme: { domain: ['#5AA454', '#A10A28', '#C7B42C', '#AAAAAA'] }
];

export const barOptions: IChartOption[] = [
    { name: "showXAxis", text: "Show X-axis", type: "boolean", value: true },
    { name: "showYAxis", text: "Show Y-axis", type: "boolean", value: true },
    { name: "showGridLines", text: "Show grid lines", type: "boolean", value: true },
    { name: "roundDomains", text: "Round domains", type: "boolean", value: true },
    { name: "showXAxisLabel", text: "Show X-axis label", type: "boolean", value: true },
    { name: "showYAxisLabel", text: "Show Y-axis label", type: "boolean", value: true },
    { name: "xAxisLabel", text: "X-axis label", type: "string", value: "x" },
    { name: "yAxisLabel", text: "Y-axis label", type: "string", value: "y" },
    { name: "schemeType", text: "Scheme type", type: "string", value: "ordinal" },
    { name: "axisMax", text: "Axis maximum", type: "number", value: 10 },
];

export const pieOptions: IChartOption[] = [
    { name: "explodeSlices", text: "Explode slices", type: "boolean", value: false },
    { name: "doughnut", text: "Doughnut", type: "boolean", value: false },
    { name: "labels", text: "Show labels", type: "boolean", value: true }
];


export const chartTypeOptions: IChartOption[][] = [
    [...barOptions, ...baseOptions],
    [...barOptions, ...baseOptions],
    [...pieOptions, ...baseOptions],
    [...barOptions, ...baseOptions],
];
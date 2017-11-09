import { INameValue } from "../shared/shared-interfaces";

export interface IChart {
    options: IChartOption[];
    data: INameValue[];
}
export interface IChartOption {
    name: string;
    text: string;
    value: any;
    type: string;
}
//export interface IBaseChartOptions {
//    //view: [700, 400],
//    animations: IChartOption;
//    scheme: any;
//    data: any[];
//    gradient: IChartOption;
//    showLegend: IChartOption;
//    legendTitle: IChartOption;
//    tooltipDisabled: IChartOption;
//}

//export interface IBarChartOptions extends IBaseChartOptions {
//    showGridLines: IChartOption;
//    roundDomains: IChartOption;
//    showXAxis: IChartOption;
//    showYAxis: IChartOption;
//    showXAxisLabel: IChartOption;
//    showYAxisLabel: IChartOption;
//    xAxisLabel: IChartOption;
//    yAxisLabel: IChartOption;
//    schemeType: IChartOption;
//}

//export interface IPieChartOptions extends IBaseChartOptions {
//    explodeSlices: IChartOption;
//    doughnut: IChartOption;
//    legend: IChartOption;
//}

//export class BaseChartOptions {
//    animations: IChartOption;
//    scheme: any;
//    data: any[];
//    gradient: IChartOption;
//    showLegend: IChartOption;
//    legendTitle: IChartOption;
//    tooltipDisabled: IChartOption;

//    constructor(options: IBaseChartOptions) {
//        this.animations = options.animations;
//        this.scheme = options.scheme;
//        this.data = options.data;
//        this.gradient = options.gradient;
//        this.showLegend = options.showLegend;
//        this.legendTitle = options.legendTitle;
//        this.tooltipDisabled = options.tooltipDisabled;
//    }
//}

//export class BarChartOptions extends BaseChartOptions {
//    showGridLines: IChartOption;
//    roundDomains: IChartOption;
//    showXAxis: IChartOption;
//    showYAxis: IChartOption;
//    showXAxisLabel: IChartOption;
//    showYAxisLabel: IChartOption;
//    xAxisLabel: IChartOption;
//    yAxisLabel: IChartOption;
//    schemeType: IChartOption;

//    constructor(baseOptions: IBaseChartOptions, options: IBarChartOptions) {
//        super(baseOptions);
//        this.showGridLines = options.showGridLines;
//        this.roundDomains = options.roundDomains;
//        this.showXAxis = options.showXAxis;
//        this.showYAxis = options.showYAxis;
//        this.showXAxisLabel = options.showXAxisLabel;
//        this.showYAxisLabel = options.showYAxisLabel;
//        this.xAxisLabel = options.xAxisLabel;
//        this.yAxisLabel = options.yAxisLabel;
//        this.schemeType = options.schemeType;
//    }
//}

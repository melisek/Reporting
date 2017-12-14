import { INameValue } from "../shared/shared-interfaces";

export interface IChart {
    options: IChartOption[];
    data: any[];
}
export interface IChartOption {
    name: string;
    text: string;
    value: any;
    type: string;
}

export interface IChartStyle {
    reportGUID: string;
    style: string;
}
export interface IEntityWithIdName {
    id: string;
    name: string;
}

export interface IResponseResult {
    status: number;
    ok: boolean;
    statusText: string;
    _body: any;
}

export interface INameValue {
    value: number;
    name: string;
}

export interface ISeriesNameValue {
    series: INameValue[];
    name: string;
}

export interface IChartDiscreteDataOptions {
    reportGUID: string;
    nameColumn: string;
    valueColumn: string;
    aggregation: number;
}

export interface IColumnSort {
    columnName: string;
    direction: string;
}

export interface IListFilter {
    filter: string;
    page: number;
    rows: number;
    sort: IColumnSort;
}
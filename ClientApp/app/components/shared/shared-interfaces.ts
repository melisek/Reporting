export interface IEntityWithIdName {
    id: number;
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
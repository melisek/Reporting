import { IColumnSort } from "../shared/shared-interfaces";

export interface IQueryColumns {
    queryGUID: string;
    columns: IQueryColumn[];
}

export interface IQuery {
    queryGUID: string;
    name: string;
}

export interface IQueryColumn {
    name: string;
    text: string;
    type: string;
}

export interface IQuerySourceDataFilter {
    queryGUID: string;
    rows: number;
    page: number;
    filter: string;
    columns: string[];
    sort: IColumnSort;
}

export interface IQuerySourceData {
    TotalCount: number;
    Data: any[];
}
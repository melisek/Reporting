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
    y: number;
    x: number;
}

export interface IQuerySourceData {
    data: any[];
}
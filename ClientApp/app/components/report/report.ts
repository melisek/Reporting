import { IEntityWithIdName } from "../shared/shared-interfaces";

export interface IReportList {
    totalCount: number;
    reports: IReport[];
}

export interface IReport {
    id: number;
    name: string;
    query: IEntityWithIdName;
    createdAt: string;
    createdBy: IEntityWithIdName;
    modifiedAt: string;
    modifiedBy: IEntityWithIdName;
    style: string;
}

export interface IReportCreate {
    name: string;
    queryGUID: string;
    columns: string[];
    filter: string;
    rows: number;
    sort: string;
    //Style: string;
}

export interface IColumnSort {
    Column: string;
    Direction: string;
}

export class Report implements IReport {
    constructor(public id: number,
        public name: string,
        public query: IEntityWithIdName,
        public createdAt: string,
        public createdBy: IEntityWithIdName,
        public modifiedAt: string,
        public modifiedBy: IEntityWithIdName,
        public style: string) {
    }

    // műveletek
}
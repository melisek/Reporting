import { IEntityWithIdName } from "../shared/shared-interfaces";

export interface IReportList {
    totalCount: number;
    reports: IReport[];
}

export interface IReport {
    reportGUID: string;
    name: string;
    query: IEntityWithIdName;
    creationDate: Date;
    author: IEntityWithIdName;
    modifyDate: Date;
    lastModifier: IEntityWithIdName;
    //style: string;
}

export interface IReportCreate {
    name: string;
    queryGUID: string;
    columns: string[];
    filter: string;
    rows: number;
    sort: IColumnSort;
    //Style: string;
}

export interface IColumnSort {
    columnName: string;
    direction: string;
}

//export class Report implements IReport {
//    constructor(public id: number,
//        public name: string,
//        public query: IEntityWithIdName,
//        public createdAt: string,
//        public createdBy: IEntityWithIdName,
//        public modifiedAt: string,
//        public modifiedBy: IEntityWithIdName,
//        public style: string) {
//    }

//    // műveletek
//}
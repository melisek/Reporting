import { IEntityWithIdName, IColumnSort } from "../shared/shared-interfaces";

export interface IDashboardList {
    totalCount: number;
    dashboards: IDashboard[];
}

export interface IDashboard {
    reportGUID: string;
    name: string;
    creationDate: Date;
    author: IEntityWithIdName;
    modifyDate: Date;
    lastModifier: IEntityWithIdName;
    //style: string;
}

export interface IDashboardCreate {
    name: string;
    //dashboardGUID: string;
    reports: IDashboardReport[];
}

export interface IDashboardReport {
    name: string;
    reportGUID: string;
    position: number;
}
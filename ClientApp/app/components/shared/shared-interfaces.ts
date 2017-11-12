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
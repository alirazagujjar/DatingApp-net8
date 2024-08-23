export interface Pagination {
    currentPage: number;
    itemsPerPage: number;
    totaleItmes: number;
    totalPage: number;
}

export class PaginationResult<T>{
    items?:T;
    pagination?:Pagination;
}
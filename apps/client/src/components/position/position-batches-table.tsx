import {
    type ColumnDef,
    type ColumnFiltersState,
    flexRender,
    getCoreRowModel,
    getFilteredRowModel,
    getPaginationRowModel,
    getSortedRowModel,
    type SortingState,
    useReactTable,
    type VisibilityState,
} from '@tanstack/react-table';
import * as React from 'react';

import { useGetPositionBatches } from '@/api/position-api';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from '@/components/ui/table';
import type { PositionBatchSummary } from '@/types/position-types';
import { timeAgo } from '@/util/time-ago';
import { useNavigate } from 'react-router-dom';
import { Spinner } from '../ui/spinner';

export function PositionBatchesTable() {
    const navigate = useNavigate();
    const [sorting, setSorting] = React.useState<SortingState>([]);
    const [columnFilters, setColumnFilters] =
        React.useState<ColumnFiltersState>([]);
    const [columnVisibility, setColumnVisibility] =
        React.useState<VisibilityState>({});
    const [rowSelection, setRowSelection] = React.useState({});

    const { data, isLoading, isError } = useGetPositionBatches();

    const columns: ColumnDef<PositionBatchSummary>[] = [
        {
            accessorKey: 'designationName',
            header: () => <div className="min-w-[150px] ml-4">Designation</div>,
            cell: ({ row }) => (
                <div className="font-medium pl-4">
                    <span
                        className="hover:cursor-pointer hover:underline"
                        onClick={(e) => {
                            e.stopPropagation();
                            navigate('/configuration/designations');
                        }}
                    >
                        {row.original.designationName}
                    </span>
                </div>
            ),
        },
        {
            id: 'location',
            header: () => <div className="min-w-[150px]">Location</div>,
            cell: ({ row }) => (
                <div className="font-medium">{row.original.jobLocation}</div>
            ),
        },
        {
            id: 'ctc-range',
            header: () => <div className="min-w-[110px]">CTC Range</div>,
            cell: ({ row }) => (
                <div className="font-medium">
                    {row.original.minCTC} - {row.original.maxCTC} LPA
                </div>
            ),
        },
        {
            id: 'positions',
            header: () => (
                <div className="w-[180px]">Positions (closed/total)</div>
            ),
            cell: ({ row }) => (
                <div className="font-medium">
                    {row.original.closedPositionsCount} /{' '}
                    {row.original.positionsCount}
                </div>
            ),
        },
        {
            id: 'positions',
            header: () => <div className="w-[80px]">Status</div>,
            cell: ({ row }) => (
                <div className="font-medium">
                    {row.original.closedPositionsCount <
                    row.original.positionsCount
                        ? 'Open'
                        : 'Closed'}
                </div>
            ),
        },
        {
            id: 'created-at',
            header: () => <div className="min-w-[150px]">Created</div>,
            cell: ({ row }) => (
                <div className="font-medium">
                    {timeAgo(row.original.createdAt)}
                </div>
            ),
        },
    ];

    const table = useReactTable({
        data:
            (data?.sort(
                (a, b) =>
                    new Date(b.createdAt).getTime() -
                    new Date(a.createdAt).getTime()
            ) as PositionBatchSummary[]) ?? [],
        columns,
        onSortingChange: setSorting,
        onColumnFiltersChange: setColumnFilters,
        getCoreRowModel: getCoreRowModel(),
        getPaginationRowModel: getPaginationRowModel(),
        getSortedRowModel: getSortedRowModel(),
        getFilteredRowModel: getFilteredRowModel(),
        onColumnVisibilityChange: setColumnVisibility,
        onRowSelectionChange: setRowSelection,
        state: {
            sorting,
            columnFilters,
            columnVisibility,
            rowSelection,
        },
    });

    if (isLoading)
        return (
            <div className="h-[50vh] flex justify-center items-center">
                <Spinner className="size-8" />
            </div>
        );
    if (isError) return <div>Error Loading Position Batches</div>;

    return (
        <div className="w-full">
            {/* header */}
            <div className="flex items-center py-4">
                <Input
                    placeholder="Filter position Batches with designation..."
                    value={
                        (table
                            .getColumn('designationName')
                            ?.getFilterValue() as string) ?? ''
                    }
                    onChange={(event) =>
                        table
                            .getColumn('designationName')
                            ?.setFilterValue(event.target.value)
                    }
                    className="max-w-sm"
                />
            </div>

            {/* table */}
            <div className="overflow-hidden rounded-md border">
                <Table>
                    <TableHeader className="bg-border">
                        {table.getHeaderGroups().map((headerGroup) => (
                            <TableRow key={headerGroup.id}>
                                {headerGroup.headers.map((header, index) => (
                                    <TableHead key={index}>
                                        {(() => {
                                            if (header.isPlaceholder) {
                                                return null;
                                            }
                                            return flexRender(
                                                header.column.columnDef.header,
                                                header.getContext()
                                            );
                                        })()}
                                    </TableHead>
                                ))}
                            </TableRow>
                        ))}
                    </TableHeader>
                    <TableBody>
                        {table.getRowModel().rows?.length ? (
                            table.getRowModel().rows.map((row) => (
                                <TableRow
                                    key={row.id}
                                    onClick={() =>
                                        navigate(
                                            `/positions/batch/${row.original.batchId}`
                                        )
                                    }
                                    data-state={
                                        row.getIsSelected() && 'selected'
                                    }
                                >
                                    {row
                                        .getVisibleCells()
                                        .map((cell, index) => (
                                            <TableCell key={index}>
                                                {flexRender(
                                                    cell.column.columnDef.cell,
                                                    cell.getContext()
                                                )}
                                            </TableCell>
                                        ))}
                                </TableRow>
                            ))
                        ) : (
                            <TableRow>
                                <TableCell
                                    colSpan={columns.length}
                                    className="h-24 text-center"
                                >
                                    No Position Batches Found.
                                </TableCell>
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
            </div>

            {/* footer */}
            <div className="flex items-center justify-between space-x-2 py-4">
                <div className="text-muted-foreground flex grow justify-end text-sm mr-5 whitespace-nowrap">
                    <p
                        className="text-muted-foreground text-sm whitespace-nowrap"
                        aria-live="polite"
                    >
                        <span className="text-foreground">
                            {table.getState().pagination.pageIndex *
                                table.getState().pagination.pageSize +
                                1}
                            -
                            {Math.min(
                                Math.max(
                                    table.getState().pagination.pageIndex *
                                        table.getState().pagination.pageSize +
                                        table.getState().pagination.pageSize,
                                    0
                                ),
                                table.getRowCount()
                            )}
                        </span>{' '}
                        of{' '}
                        <span className="text-foreground">
                            {table.getRowCount().toString()}
                        </span>
                    </p>
                </div>

                <div className="space-x-2">
                    <Button
                        variant="outline"
                        size="sm"
                        onClick={() => table.previousPage()}
                        disabled={!table.getCanPreviousPage()}
                    >
                        Previous
                    </Button>
                    <Button
                        variant="outline"
                        size="sm"
                        onClick={() => table.nextPage()}
                        disabled={!table.getCanNextPage()}
                    >
                        Next
                    </Button>
                </div>
            </div>
        </div>
    );
}

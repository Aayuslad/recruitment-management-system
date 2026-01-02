/* eslint-disable indent */
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
import { MoreHorizontal } from 'lucide-react';
import * as React from 'react';

import { useGetBatchPositions } from '@/api/position-api';
import { Button } from '@/components/ui/button';
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuLabel,
    DropdownMenuSeparator,
    DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from '@/components/ui/table';
import type { BatchPositionsSummary } from '@/types/position-types';
import { useNavigate } from 'react-router-dom';
import { Spinner } from '../ui/spinner';
import { POSITION_STATUS } from '@/types/enums';

export function BatchPositionsTable({ batchId }: { batchId: string }) {
    const navigate = useNavigate();
    const [sorting, setSorting] = React.useState<SortingState>([]);
    const [columnFilters, setColumnFilters] =
        React.useState<ColumnFiltersState>([]);
    const [columnVisibility, setColumnVisibility] =
        React.useState<VisibilityState>({});
    const [rowSelection, setRowSelection] = React.useState({});

    const { data, isLoading, isError } = useGetBatchPositions(batchId);

    const columns: ColumnDef<BatchPositionsSummary>[] = [
        // {
        //     id: 'position-id',
        //     header: () => {
        //         return <div className="ml-2">Position Id</div>;
        //     },
        //     cell: ({ row }) => (
        //         <div className="ml-2 from-sm font-mono w-[70px]">
        //             {row.original.positionId.slice(0, 6).toUpperCase()}...
        //             <button
        //                 onClick={() => {
        //                     navigator.clipboard.writeText(
        //                         row.original.positionId
        //                     );
        //                     toast.success('Copied to clipboard');
        //                 }}
        //                 className="text-muted-foreground hover:text-foreground hover:cursor-pointer"
        //                 title="Copy full ID"
        //             >
        //                 <Copy size={16} />
        //             </button>
        //         </div>
        //     ),
        // },
        {
            id: 'index',
            header: () => {
                return <div className="w-[20px] ml-4 ">Index</div>;
            },
            cell: ({ row }) => (
                <div className="font-medium w-min ml-4">{row.index + 1}</div>
            ),
        },
        {
            id: 'status',
            header: () => {
                return <div className="w-min ">Status</div>;
            },
            cell: ({ row }) => (
                <div className="font-medium w-min">{row.original.status}</div>
            ),
        },
        {
            id: 'candidate-or-closure-reason',
            header: () => {
                return (
                    <div className="w-[170px] -mr-5">
                        Candidate Name / Closure Reason
                    </div>
                );
            },
            cell: ({ row }) => (
                <div className="font-medium">
                    {row.original.closedByCandidateFullName !== null &&
                    row.original.closedByCandidateFullName !== undefined ? (
                        <span className="text-muted-foreground">
                            {row.original.closedByCandidateFullName}
                        </span>
                    ) : row.original.closureReason !== null &&
                      row.original.closureReason !== undefined ? (
                        <span className="text-muted-foreground">
                            {row.original.closureReason}
                        </span>
                    ) : (
                        <span>–</span>
                    )}
                </div>
            ),
        },
        {
            id: 'actions',
            enableHiding: false,
            cell: () => {
                return (
                    <DropdownMenu>
                        <DropdownMenuTrigger asChild className="-mr-4 ">
                            <Button variant="ghost" className="h-8 w-8 p-0">
                                <span className="sr-only">Open menu</span>
                                <MoreHorizontal />
                            </Button>
                        </DropdownMenuTrigger>
                        {/* // TODO: complete these actions */}
                        <DropdownMenuContent align="end">
                            <DropdownMenuLabel>Actions</DropdownMenuLabel>
                            <DropdownMenuSeparator />
                            <DropdownMenuItem>Put On Hold</DropdownMenuItem>
                            <DropdownMenuItem>Delete</DropdownMenuItem>
                            <DropdownMenuItem>Close</DropdownMenuItem>
                        </DropdownMenuContent>
                    </DropdownMenu>
                );
            },
        },
    ];

    const table = useReactTable({
        data: [
            ...(data?.filter((x) => x.status === POSITION_STATUS.ON_HOLD) ??
                []),
            ...(data?.filter((x) => x.status === POSITION_STATUS.CLOSED) ?? []),
            ...(data?.filter((x) => x.status === POSITION_STATUS.OPEN) ?? []),
        ] as BatchPositionsSummary[],
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
            <div className="w-full h-[30vh] flex justify-center items-center">
                <Spinner className="size-8" />
            </div>
        );
    if (isError)
        return (
            <div className="w-full h-[30vh] flex justify-center items-center">
                Error Loading Position Batches
            </div>
        );

    return (
        <div className="w-[540px] mx-auto">
            {/* table */}
            <div className="overflow-hidden rounded-md border">
                <Table>
                    <TableHeader>
                        {table.getHeaderGroups().map((headerGroup) => (
                            <TableRow key={headerGroup.id}>
                                {headerGroup.headers.map((header) => (
                                    <TableHead key={header.id}>
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
                                    {row.getVisibleCells().map((cell) => (
                                        <TableCell key={cell.id}>
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
            <div className="flex items-center justify-end space-x-2 py-4">
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

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

import { useGetJobOpenings } from '@/api/job-opening-api';
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
import type { JobOpeningSummary } from '@/types/job-opening-types';
import { timeAgo } from '@/util/time-ago';
import { useNavigate } from 'react-router-dom';
import { Spinner } from '../ui/spinner';

export function JobOpeningsTable() {
    const navigate = useNavigate();
    const [sorting, setSorting] = React.useState<SortingState>([]);
    const [columnFilters, setColumnFilters] =
        React.useState<ColumnFiltersState>([]);
    const [columnVisibility, setColumnVisibility] =
        React.useState<VisibilityState>({});
    const [rowSelection, setRowSelection] = React.useState({});

    const { data, isLoading, isError } = useGetJobOpenings();

    const columns: ColumnDef<JobOpeningSummary>[] = [
        {
            accessorKey: 'title',
            header: () => <div className="w-[200px] ml-4">Title</div>,
            cell: ({ row }) => (
                <div className="font-medium pl-4">{row.original.title}</div>
            ),
        },
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
        // {
        //     id: 'type',
        //     header: 'Opening Type',
        //     cell: ({ row }) => (
        //         <div className="font-medium w-[130px]">{row.original.type}</div>
        //     ),
        // },
        {
            id: 'interview-rounds',
            header: () => (
                <div className="min-w-[150px] mr-4">Interview Rounds</div>
            ),
            cell: ({ row }) => (
                <div className="font-medium min-w-[150px] mr-4">
                    {`${row.original.interviewRounds.length} Rounds ${
                        row.original.interviewRounds.length === 0
                            ? ''
                            : `(${row.original.interviewRounds
                                  .sort((a, b) => b.roundNumber - a.roundNumber)
                                  .reverse()
                                  .map((round) => round.type)
                                  .join(' -> ')})`
                    }`}
                </div>
            ),
        },
        {
            id: 'applications-count',
            header: () => <div className="min-w-[110px]">Applications</div>,
            cell: ({ row }) => (
                <div className="font-medium pl-7">
                    {row.original.applicationsCount}
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
            ) as JobOpeningSummary[]) ?? [],
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
    if (isError) return <div>Error Loading Job Openings</div>;

    return (
        <div className="w-full">
            <h2 className="font-semibold text-xl">Job Openings:</h2>

            {/* header */}
            <div className="flex items-center justify-between py-4">
                <Input
                    placeholder="Filter job openings with title..."
                    value={
                        (table
                            .getColumn('title')
                            ?.getFilterValue() as string) ?? ''
                    }
                    onChange={(event) =>
                        table
                            .getColumn('title')
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
                                            `/job-openings/opening/${row.original.id}`
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
                                    No Job Openings Found.
                                </TableCell>
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
            </div>

            {/* footer */}
            <div className="flex items-center justify-end space-x-2 py-4">
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

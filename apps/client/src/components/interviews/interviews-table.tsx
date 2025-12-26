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

import { useGetAssignedInterviews } from '@/api/interviews-api';
import { Button } from '@/components/ui/button';
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from '@/components/ui/table';
import type { InterviewSummary } from '@/types/interview-types';
import { durationFormatConverter } from '@/util/interview-round-duration-format-converter';
import { useNavigate } from 'react-router-dom';
import { Spinner } from '../ui/spinner';

export function InterviewsTable() {
    const navigate = useNavigate();
    const [sorting, setSorting] = React.useState<SortingState>([]);
    const [columnFilters, setColumnFilters] =
        React.useState<ColumnFiltersState>([]);
    const [columnVisibility, setColumnVisibility] =
        React.useState<VisibilityState>({});
    const [rowSelection, setRowSelection] = React.useState({});

    const { data, isLoading, isError } = useGetAssignedInterviews();

    const columns: ColumnDef<InterviewSummary>[] = [
        {
            accessorKey: 'candidate-name',
            header: () => <div className="w-[220px] ml-4">Candidate Name</div>,
            cell: ({ row }) => (
                <div className="font-medium pl-4">
                    {row.original.candidateName}
                </div>
            ),
        },
        {
            accessorKey: 'designation-name',
            header: () => <div className="w-[180px]">Designation</div>,
            cell: ({ row }) => (
                <div
                    className="font-medium hover:cursor-pointer hover:underline"
                    onClick={() => navigate('/configuration/designations')}
                >
                    {row.original.designationName}
                </div>
            ),
        },
        {
            id: 'round',
            header: () => <div className="w-[120px]">Round</div>,
            cell: ({ row }) => (
                <div className="font-medium w-[130px]">
                    {row.original.roundNumber}. {row.original.interviewType}
                </div>
            ),
        },
        {
            id: 'duration',
            header: () => <div className="w-[120px]">Duration</div>,
            cell: ({ row }) => (
                <div className="font-medium w-[150px]">
                    {durationFormatConverter(row.original.durationInMinutes)}
                </div>
            ),
        },
        {
            id: 'status',
            header: () => <div className="w-[120px]">Status</div>,
            cell: ({ row }) => (
                <div className="font-medium">{row.original.status}</div>
            ),
        },
        {
            accessorKey: 'sheduled-At',
            header: () => <div className="w-[150px] ml-4">Sheduled At</div>,
            cell: ({ row }) => (
                <div>
                    {row.original.scheduledAt
                        ? new Date(row.original.scheduledAt).toLocaleString()
                        : '-'}
                </div>
            ),
        },
    ];

    const table = useReactTable({
        data: data as InterviewSummary[],
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
    if (isError) return <div>Error Loading Interviews</div>;

    return (
        <div className="w-full">
            <h2 className="font-semibold text-xl mb-4">Interviews:</h2>

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
                                            `/interviews/interview/${row.original.id}`
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
                                    No interviews Found.
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

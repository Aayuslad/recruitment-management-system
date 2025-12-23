'use client';

import { useState } from 'react';

import type {
    ColumnDef,
    ColumnFiltersState,
    SortingState,
    VisibilityState,
} from '@tanstack/react-table';
import {
    flexRender,
    getCoreRowModel,
    getFilteredRowModel,
    getPaginationRowModel,
    getSortedRowModel,
    useReactTable,
} from '@tanstack/react-table';

import { useGetJobOpeningApplications } from '@/api/job-application-api';
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from '@/components/ui/table';
import type { JobOpeningApplicationSummary } from '@/types/job-application-types';
import { useNavigate } from 'react-router-dom';

type Props = {
    jobOpeningId: string;
};

export const JobOpeningApplicationsTable = ({ jobOpeningId }: Props) => {
    const [sorting, setSorting] = useState<SortingState>([]);
    const [columnFilters, setColumnFilters] = useState<ColumnFiltersState>([]);
    const [columnVisibility, setColumnVisibility] = useState<VisibilityState>(
        {}
    );
    const [rowSelection, setRowSelection] = useState({});
    const { data } = useGetJobOpeningApplications(jobOpeningId);
    const navigate = useNavigate();

    const columns: ColumnDef<JobOpeningApplicationSummary>[] = [
        {
            accessorKey: 'candidate-name',
            header: () => {
                return <div className="w-[250px] ml-4">Candidate Name</div>;
            },
            cell: ({ row }) => (
                <div className="font-medium ml-4">
                    {row.original.candidateName}
                </div>
            ),
        },
        {
            id: 'status',
            accessorKey: 'status',
            header: () => <div className="w-[90px]">Status</div>,
            cell: ({ row }) => <div>{row.original.status}</div>,
        },
        {
            accessorKey: 'applied-at',
            header: () => <div className="w-[180px]">Applied At</div>,
            cell: ({ row }) => (
                <div>{new Date(row.original.appliedAt).toLocaleString()}</div>
            ),
        },
    ];

    const table = useReactTable({
        data: data ?? [],
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

    return (
        <div className="w-full">
            <h2 className="font-semibold text-xl mb-4">Job Applications:</h2>
            <div className="rounded-md border">
                <Table>
                    <TableHeader>
                        {table.getHeaderGroups().map((headerGroup) => (
                            <TableRow key={headerGroup.id}>
                                {headerGroup.headers.map((header) => {
                                    return (
                                        <TableHead key={header.id}>
                                            {header.isPlaceholder
                                                ? null
                                                : flexRender(
                                                      header.column.columnDef
                                                          .header,
                                                      header.getContext()
                                                  )}
                                        </TableHead>
                                    );
                                })}
                            </TableRow>
                        ))}
                    </TableHeader>
                    <TableBody>
                        {table.getRowModel().rows.length ? (
                            table.getRowModel().rows.map((row) => (
                                <TableRow
                                    key={row.id}
                                    data-state={
                                        row.getIsSelected() && 'selected'
                                    }
                                    onClick={() =>
                                        navigate(
                                            `/job-applications/application/${row.original.id}`
                                        )
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
                                    No results.
                                </TableCell>
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
            </div>
            <p className="text-muted-foreground mt-4 text-center text-sm">
                Job Applications Table
            </p>
        </div>
    );
};

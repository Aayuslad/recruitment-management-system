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

import { useGetJobApplicationsToReview } from '@/api/job-application-api';
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from '@/components/ui/table';
import type { JobApplicationSummary } from '@/types/job-application-types';
import { useNavigate } from 'react-router-dom';
import { Spinner } from '../ui/spinner';

export const JobApplicationsTable = () => {
    const [sorting, setSorting] = useState<SortingState>([]);
    const [columnFilters, setColumnFilters] = useState<ColumnFiltersState>([]);
    const [columnVisibility, setColumnVisibility] = useState<VisibilityState>(
        {}
    );
    const [rowSelection, setRowSelection] = useState({});
    const { data, isLoading, isError } = useGetJobApplicationsToReview();
    // const { currentJobApplicationTab } = useAppStore(
    //     useShallow((s) => ({
    //         currentJobApplicationTab: s.currentJobApplicationTab,
    //     }))
    // );
    const navigate = useNavigate();

    // useEffect(() => {
    //     if (!currentJobApplicationTab || currentJobApplicationTab === 'All') {
    //         setColumnFilters([]);
    //         return;
    //     }

    //     setColumnFilters([
    //         {
    //             id: 'status',
    //             value: currentJobApplicationTab,
    //         },
    //     ]);
    // }, [currentJobApplicationTab]);

    const columns: ColumnDef<JobApplicationSummary>[] = [
        // {
        //     id: 'select',
        //     header: ({ table }) => (
        //         <Checkbox
        //             checked={
        //                 table.getIsAllPageRowsSelected() ||
        //                 (table.getIsSomePageRowsSelected() && 'indeterminate')
        //             }
        //             onCheckedChange={(value) =>
        //                 table.toggleAllPageRowsSelected(!!value)
        //             }
        //             aria-label="Select all"
        //         />
        //     ),
        //     cell: ({ row }) => (
        //         <Checkbox
        //             checked={row.getIsSelected()}
        //             onCheckedChange={(value) => row.toggleSelected(!!value)}
        //             aria-label="Select row"
        //         />
        //     ),
        //     enableSorting: false,
        //     enableHiding: false,
        // },
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
            accessorKey: 'designation-name',
            header: () => <div className="w-[150px]">Designation</div>,
            cell: ({ row }) => <div>{row.original.designation}</div>,
        },
        {
            accessorKey: 'avg-rating',
            header: () => <div className="w-[100px]">Avg Rating</div>,
            cell: ({ row }) => <div>{row.original.avgRating || '-'}</div>,
        },
        {
            id: 'status',
            accessorKey: 'status',
            header: () => <div className="w-[90px]">Status</div>,
            cell: ({ row }) => <div>{row.original.status}</div>,
            // filterFn: (row, columnId, filterValue) => {
            //     return row.getValue(columnId) === filterValue;
            // },
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

    if (isLoading)
        return (
            <div className="h-[50vh] flex justify-center items-center">
                <Spinner className="size-8" />
            </div>
        );
    if (isError) return <div>Error Loading Job Openings</div>;

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
                                            `/application-reviews/application/${row.original.id}`
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

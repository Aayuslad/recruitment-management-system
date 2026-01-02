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
import {
    JOB_APPLICATION_STATUS,
    type JobApplicationStatus,
} from '@/types/enums';
import { Button } from '../ui/button';
import { Label } from '../ui/label';
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from '../ui/select';
import { timeAgo } from '@/util/time-ago';
import { Tabs, TabsList, TabsTrigger } from '../ui/tabs';

type Props = {
    jobOpeningId: string;
};

type JobApplicationTab = { name: string; statesToShow: JobApplicationStatus[] };

const tabs: JobApplicationTab[] = [
    {
        name: 'All',
        statesToShow: [...Object.values(JOB_APPLICATION_STATUS)],
    },
    {
        name: 'Pending Screening',
        statesToShow: [JOB_APPLICATION_STATUS.APPLIED],
    },
    {
        name: 'Interviewing',
        statesToShow: [
            JOB_APPLICATION_STATUS.SHORTLISTED,
            JOB_APPLICATION_STATUS.INTERVIEWED,
        ],
    },
    {
        name: 'Finalised',
        statesToShow: [
            JOB_APPLICATION_STATUS.OFFERED,
            JOB_APPLICATION_STATUS.HIRED,
        ],
    },
    {
        name: 'Rejected',
        statesToShow: [JOB_APPLICATION_STATUS.REJECTED],
    },
    {
        name: 'On Hold',
        statesToShow: [JOB_APPLICATION_STATUS.ON_HOLD],
    },
];

export const JobOpeningApplicationsTable = ({ jobOpeningId }: Props) => {
    const [sorting, setSorting] = useState<SortingState>([]);
    const [columnFilters, setColumnFilters] = useState<ColumnFiltersState>([]);
    const [columnVisibility, setColumnVisibility] = useState<VisibilityState>(
        {}
    );
    const [rowSelection, setRowSelection] = useState({});
    const { data } = useGetJobOpeningApplications(jobOpeningId);
    const [activeApplicationTab, setActiveApplicationTab] =
        useState<JobApplicationTab>(tabs[0]);
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
            header: () => <div className="w-[150px]">Applied</div>,
            cell: ({ row }) => <div>{timeAgo(row.original.appliedAt)}</div>,
        },
    ];

    const table = useReactTable({
        data:
            data?.sort(
                (a, b) =>
                    new Date(b.appliedAt).getTime() -
                    new Date(a.appliedAt).getTime()
            ) ?? [],
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

            <div className="w-full mb-4">
                <Tabs
                    defaultValue="explore"
                    className="gap-4"
                    value={activeApplicationTab.name}
                    onValueChange={(value) => {
                        setActiveApplicationTab(
                            tabs.find((x) => x.name === value) ?? tabs[0]
                        );
                    }}
                >
                    <TabsList className="bg-background rounded-none border-b p-0">
                        {tabs.map((tab) => (
                            <TabsTrigger
                                key={tab.name}
                                value={tab.name}
                                className="bg-background px-4 data-[state=active]:border-primary dark:data-[state=active]:border-primary h-full rounded-none border-0 border-b-2 border-transparent data-[state=active]:shadow-none"
                            >
                                {tab.name} (
                                {
                                    data?.filter((x) =>
                                        tab.statesToShow.includes(x.status)
                                    ).length
                                }
                                )
                            </TabsTrigger>
                        ))}
                    </TabsList>
                </Tabs>
            </div>

            <div className="rounded-md border mx-4">
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
                        {table
                            .getRowModel()
                            .rows.filter((x) =>
                                activeApplicationTab.statesToShow.includes(
                                    x.original.status
                                )
                            ).length ? (
                            table
                                .getRowModel()
                                .rows.filter((x) =>
                                    activeApplicationTab.statesToShow.includes(
                                        x.original.status
                                    )
                                )
                                .map((row) => (
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

            {/* footer */}
            <div className="flex items-center justify-between space-x-2 py-4">
                <div className="flex items-center gap-3 ml-1">
                    <Label className="max-sm:sr-only font-normal">
                        Rows per page
                    </Label>
                    <Select
                        value={table.getState().pagination.pageSize.toString()}
                        onValueChange={(value) => {
                            table.setPageSize(Number(value));
                        }}
                    >
                        <SelectTrigger className="w-fit whitespace-nowrap">
                            <SelectValue placeholder="Select number of results" />
                        </SelectTrigger>
                        <SelectContent className="[&_*[role=option]]:pr-8 [&_*[role=option]]:pl-2 [&_*[role=option]>span]:right-2 [&_*[role=option]>span]:left-auto">
                            {[5, 10, 15, 20].map((pageSize) => (
                                <SelectItem
                                    key={pageSize}
                                    value={pageSize.toString()}
                                >
                                    {pageSize}
                                </SelectItem>
                            ))}
                        </SelectContent>
                    </Select>
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
};

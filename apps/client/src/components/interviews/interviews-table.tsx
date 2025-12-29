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
import { INTERVIEW_STATUS, type InterviewStatus } from '@/types/enums';
import { Tabs, TabsList, TabsTrigger } from '../ui/tabs';
import { Label } from '../ui/label';
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from '../ui/select';

type TabType = { name: string; statesToShow: InterviewStatus[] };

const tabs: TabType[] = [
    {
        name: 'Upcoming',
        statesToShow: [INTERVIEW_STATUS.SCHEDULED],
    },
    {
        name: 'To Be Scheduled',
        statesToShow: [INTERVIEW_STATUS.NOT_SCHEDULED],
    },
    {
        name: 'Completed',
        statesToShow: [INTERVIEW_STATUS.COMPLETED],
    },
];

export function InterviewsTable() {
    const navigate = useNavigate();
    const [sorting, setSorting] = React.useState<SortingState>([]);
    const [columnFilters, setColumnFilters] =
        React.useState<ColumnFiltersState>([]);
    const [columnVisibility, setColumnVisibility] =
        React.useState<VisibilityState>({});
    const [rowSelection, setRowSelection] = React.useState({});
    const [activeTab, setActiveTab] = React.useState<TabType>(tabs[0]);

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
            header: () => <div className=" min-w-[150px]">Designation</div>,
            cell: ({ row }) => (
                <div>
                    <span
                        className="font-medium hover:cursor-pointer hover:underline"
                        onClick={() => navigate('/configuration/designations')}
                    >
                        {row.original.designationName}
                    </span>
                </div>
            ),
        },
        {
            id: 'round',
            header: () => <div className="min-w-[120px]">Round</div>,
            cell: ({ row }) => (
                <div className="font-medium min-w-[130px]">
                    {row.original.roundNumber}. {row.original.interviewType}
                </div>
            ),
        },
        {
            id: 'duration',
            header: () => <div className="w-[150px]">Duration</div>,
            cell: ({ row }) => (
                <div className="font-medium w-[150px]">
                    {durationFormatConverter(row.original.durationInMinutes)}
                </div>
            ),
        },
        {
            accessorKey: 'scheduled-At',
            header: () => <div className="w-[160px]">Scheduled At</div>,
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
        data: data?.sort(
            (a, b) =>
                new Date(b?.scheduledAt ?? '').getTime() -
                new Date(a?.scheduledAt ?? '').getTime()
        ) as InterviewSummary[],
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
            <div className="w-full border-b flex items-center justify-center mb-6">
                <Tabs
                    defaultValue="explore"
                    className="gap-4"
                    value={activeTab.name}
                    onValueChange={(value) => {
                        setActiveTab(
                            tabs.find((x) => x.name === value) ?? tabs[0]
                        );
                    }}
                >
                    <TabsList className="bg-background rounded-none border-b p-0">
                        {tabs.map((tab) => (
                            <TabsTrigger
                                key={tab.name}
                                value={tab.name}
                                className="bg-background text-base px-8 data-[state=active]:border-primary dark:data-[state=active]:border-primary h-full rounded-none border-0 border-b-2 border-transparent data-[state=active]:shadow-none"
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
                        {table
                            .getRowModel()
                            .rows.filter((x) =>
                                activeTab.statesToShow.includes(
                                    x.original.status
                                )
                            ).length ? (
                            table
                                .getRowModel()
                                .rows.filter((x) =>
                                    activeTab.statesToShow.includes(
                                        x.original.status
                                    )
                                )
                                .map((row) => (
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
                            {[5, 10, 25, 50].map((pageSize) => (
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
}

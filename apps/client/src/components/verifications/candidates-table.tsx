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

import { useGetCandidates } from '@/api/candidate-api';
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
import type { CandidateSummary } from '@/types/candidate-types';
import { JOB_APPLICATION_STATUS } from '@/types/enums';
import { timeAgo } from '@/util/time-ago';
import { useNavigate } from 'react-router-dom';
import { Label } from '../ui/label';
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from '../ui/select';
import { Spinner } from '../ui/spinner';
import { Tabs, TabsList, TabsTrigger } from '../ui/tabs';

type TabType = {
    name: string;
};

const tabs: TabType[] = [
    {
        name: 'Pending',
    },
    {
        name: 'Cleared',
    },
    {
        name: 'Hired',
    },
];

export function CandidatesTable() {
    const navigate = useNavigate();
    const [sorting, setSorting] = React.useState<SortingState>([]);
    const [columnFilters, setColumnFilters] =
        React.useState<ColumnFiltersState>([]);
    const [columnVisibility, setColumnVisibility] =
        React.useState<VisibilityState>({});
    const [rowSelection, setRowSelection] = React.useState({});
    const [activeTab, setActiveTab] = React.useState<TabType>(tabs[0]);

    const { data, isLoading, isError } = useGetCandidates();

    const columns: ColumnDef<CandidateSummary>[] = [
        {
            accessorKey: 'name',
            header: () => <div className="min-w-[240px] ml-4">Name</div>,
            cell: ({ row }) => (
                <div className="font-medium ml-4">
                    {`${row.original.firstName} ${row.original.middleName} ${row.original.lastName}`}
                </div>
            ),
        },
        {
            accessorKey: 'collegeName',
            header: () => <div className="min-w-[150px]">College</div>,
            cell: ({ row }) => <div>{row.original.collegeName}</div>,
        },
        {
            id: 'gender',
            header: () => <div className="w-[80px]">Gender</div>,
            cell: ({ row }) => (
                <div className="font-medium">{row.original.gender}</div>
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
            ) as CandidateSummary[]) ?? [],
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
    if (isError) return <div>Error Loading Candidates</div>;

    return (
        <div className="w-full">
            <div className="w-full border-b flex items-center justify-center mb-4">
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
                                    data?.filter((candidate) => {
                                        if (tab.name === 'Pending') {
                                            const verificationStatus =
                                                candidate.isBgVerificationCompleted &&
                                                candidate.isDocumentsVerified;

                                            const isOffered =
                                                candidate.jobApplications.some(
                                                    (x) =>
                                                        x.status ===
                                                        JOB_APPLICATION_STATUS.OFFERED
                                                );

                                            return (
                                                !verificationStatus && isOffered
                                            );
                                        }

                                        if (tab.name === 'Cleared') {
                                            const verificationStatus =
                                                candidate.isBgVerificationCompleted &&
                                                candidate.isDocumentsVerified;

                                            const isOffered =
                                                candidate.jobApplications.some(
                                                    (x) =>
                                                        x.status ===
                                                        JOB_APPLICATION_STATUS.OFFERED
                                                );

                                            return (
                                                verificationStatus && isOffered
                                            );
                                        }

                                        if (tab.name === 'Hired') {
                                            const isHired =
                                                candidate.jobApplications.some(
                                                    (x) =>
                                                        x.status ===
                                                        JOB_APPLICATION_STATUS.HIRED
                                                );

                                            return isHired;
                                        }
                                    }).length
                                }
                                )
                            </TabsTrigger>
                        ))}
                    </TabsList>
                </Tabs>
            </div>

            {/* header */}
            <div className="flex items-center justify-between py-4">
                <Input
                    placeholder="Filter candidates with name..."
                    value={
                        (table.getColumn('name')?.getFilterValue() as string) ??
                        ''
                    }
                    onChange={(event) =>
                        table
                            .getColumn('name')
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
                        {/* // TODO: refactor this. it can be compressed */}
                        {table.getRowModel().rows.filter((row) => {
                            if (activeTab.name === 'Pending') {
                                const verificationStatus =
                                    row.original.isBgVerificationCompleted &&
                                    row.original.isDocumentsVerified;

                                const isOffered =
                                    row.original.jobApplications.some(
                                        (x) =>
                                            x.status ===
                                            JOB_APPLICATION_STATUS.OFFERED
                                    );

                                return !verificationStatus && isOffered;
                            }

                            if (activeTab.name === 'Cleared') {
                                const verificationStatus =
                                    row.original.isBgVerificationCompleted &&
                                    row.original.isDocumentsVerified;

                                const isOffered =
                                    row.original.jobApplications.some(
                                        (x) =>
                                            x.status ===
                                            JOB_APPLICATION_STATUS.OFFERED
                                    );

                                return verificationStatus && isOffered;
                            }

                            if (activeTab.name === 'Hired') {
                                const isHired =
                                    row.original.jobApplications.some(
                                        (x) =>
                                            x.status ===
                                            JOB_APPLICATION_STATUS.HIRED
                                    );

                                return isHired;
                            }
                        }).length ? (
                            table
                                .getRowModel()
                                .rows.filter((row) => {
                                    if (activeTab.name === 'Pending') {
                                        const verificationStatus =
                                            row.original
                                                .isBgVerificationCompleted &&
                                            row.original.isDocumentsVerified;

                                        const isOffered =
                                            row.original.jobApplications.some(
                                                (x) =>
                                                    x.status ===
                                                    JOB_APPLICATION_STATUS.OFFERED
                                            );

                                        return !verificationStatus && isOffered;
                                    }

                                    if (activeTab.name === 'Cleared') {
                                        const verificationStatus =
                                            row.original
                                                .isBgVerificationCompleted &&
                                            row.original.isDocumentsVerified;

                                        const isOffered =
                                            row.original.jobApplications.some(
                                                (x) =>
                                                    x.status ===
                                                    JOB_APPLICATION_STATUS.OFFERED
                                            );

                                        return verificationStatus && isOffered;
                                    }

                                    if (activeTab.name === 'Hired') {
                                        const isHired =
                                            row.original.jobApplications.some(
                                                (x) =>
                                                    x.status ===
                                                    JOB_APPLICATION_STATUS.HIRED
                                            );

                                        return isHired;
                                    }
                                })
                                .map((row) => (
                                    <TableRow
                                        key={row.id}
                                        onClick={() =>
                                            navigate(
                                                `/verifications/candidate/${row.original.id}`
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
                                    No Candidates Found.
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

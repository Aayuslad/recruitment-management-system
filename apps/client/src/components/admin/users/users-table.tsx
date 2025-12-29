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
import { ChevronDown } from 'lucide-react';
import * as React from 'react';

import { useGetUsers } from '@/api/user-api';
import { Button } from '@/components/ui/button';
import {
    DropdownMenu,
    DropdownMenuCheckboxItem,
    DropdownMenuContent,
    DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import { Input } from '@/components/ui/input';
import { Spinner } from '@/components/ui/spinner';
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from '@/components/ui/table';
import type { UsersDetail } from '@/types/user-types';
import { useAppStore } from '@/store';
import { useShallow } from 'zustand/react/shallow';

export function UsersTable() {
    const [sorting, setSorting] = React.useState<SortingState>([]);
    const [columnFilters, setColumnFilters] =
        React.useState<ColumnFiltersState>([]);
    const [columnVisibility, setColumnVisibility] =
        React.useState<VisibilityState>({});
    const [rowSelection, setRowSelection] = React.useState({});
    const { openUserEditDialog } = useAppStore(
        useShallow((s) => ({
            openUserEditDialog: s.openUserEditDialog,
        }))
    );

    const { data, isLoading, isError } = useGetUsers();

    const columns: ColumnDef<UsersDetail>[] = [
        {
            id: 'user-name',
            header: () => <div className="w-[120px] ml-4">User Name</div>,
            cell: ({ row }) => {
                return <div className="ml-4">{row.original.userName}</div>;
            },
        },
        {
            accessorKey: 'name',
            header: () => <div className="w-[200px]">Full Name</div>,
            cell: ({ row }) => (
                <div className="font-medium ">
                    {`${row.original.firstName} ${row.original.middleName} ${row.original.lastName}`}
                </div>
            ),
        },
        {
            accessorKey: 'email',
            header: () => <div className="w-[200px]">Email</div>,
            cell: ({ row }) => {
                return <div>{row.original.email}</div>;
            },
        },
        // {
        //     accessorKey: 'contact-number',
        //     header: () => <div className="w-[140px]">Contact Number</div>,
        //     cell: ({ row }) => {
        //         return <div>{row.original.contactNumber}</div>;
        //     },
        // },
        // {
        //     accessorKey: 'gender',
        //     header: () => <div className="w-[120px]">Gender</div>,
        //     cell: ({ row }) => {
        //         return <div>{row.original.gender}</div>;
        //     },
        // },
        {
            accessorKey: 'roles',
            header: () => <div className="w-[200px]">Roles</div>,
            cell: ({ row }) => {
                return (
                    <div>
                        {row.original.roles.map((role) => role.name).join(', ')}
                    </div>
                );
            },
        },
        {
            accessorKey: 'actions',
            header: () => <div className="w-[100px]">Actions</div>,
            cell: ({ row }) => {
                return (
                    <div>
                        <button
                            className="text-yellow-500 cursor-pointer font-semibold"
                            onClick={() => {
                                openUserEditDialog(row.original);
                            }}
                        >
                            Manage Roles
                        </button>
                    </div>
                );
            },
        },
    ];

    const table = useReactTable({
        data: data as UsersDetail[],
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

    if (isLoading) {
        return (
            <div className="w-full h-[50vh] flex justify-center items-center">
                <Spinner className="size-8" />
            </div>
        );
    }

    if (isError) {
        return (
            <div className="w-full h-[50vh] flex justify-center items-center">
                Error Loading Users
            </div>
        );
    }

    return (
        <div className="">
            {/* header */}
            <div className="flex items-center py-4">
                <Input
                    placeholder="Filter users by name..."
                    value={
                        (table.getColumn('name')?.getFilterValue() as string) ??
                        ''
                    }
                    onChange={(event) =>
                        table
                            .getColumn('name')
                            ?.setFilterValue(event.target.value)
                    }
                    className="w-[250px]"
                />
                <DropdownMenu>
                    <DropdownMenuTrigger asChild>
                        <Button variant="outline" className="ml-auto">
                            Columns <ChevronDown className="ml-2 h-4 w-4" />
                        </Button>
                    </DropdownMenuTrigger>
                    <DropdownMenuContent align="end">
                        {table
                            .getAllColumns()
                            .filter((column) => column.getCanHide())
                            .map((column) => (
                                <DropdownMenuCheckboxItem
                                    key={column.id}
                                    className="capitalize"
                                    checked={column.getIsVisible()}
                                    onCheckedChange={(value) =>
                                        column.toggleVisibility(!!value)
                                    }
                                >
                                    {column.id}
                                </DropdownMenuCheckboxItem>
                            ))}
                    </DropdownMenuContent>
                </DropdownMenu>
            </div>

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
                        {table?.getRowModel().rows?.length ? (
                            table.getRowModel().rows.map((row) => (
                                <TableRow
                                    key={row.id}
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
                                    No users Found.
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
